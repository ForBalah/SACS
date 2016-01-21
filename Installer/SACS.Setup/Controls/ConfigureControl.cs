using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;
using SACS.Setup.Classes;
using SACS.Setup.Config;
using SACS.Setup.Forms;

namespace SACS.Setup.Controls
{
    /// <summary>
    /// The configure SACS setup control
    /// </summary>
    public partial class ConfigureControl : UserControl
    {
        #region Fields

        private bool _hasServerPropertyChanges = false;
        private ServerConfigFile _serverConfig = new ServerConfigFile();

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureControl"/> class.
        /// </summary>
        public ConfigureControl()
        {
            InitializeComponent();
        }

        #endregion Constructors and Destructors

        #region Event Handlers

        /// <summary>
        /// Handles the VisibleChanged event of the ConfigureControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ConfigureControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.RefreshServerLogin();
                this.LoadServerConfig();
            }
        }

        /// <summary>
        /// Handles the Click event of the DeployScriptsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeployScriptsButton_Click(object sender, EventArgs e)
        {
            var deployDialog = new SqlDeployForm();
            deployDialog.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the RefreshAccountButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RefreshAccountButton_Click(object sender, EventArgs e)
        {
            this.RefreshServerLogin();
        }

        /// <summary>
        /// Handles the Click event of the ServerApplyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServerApplyButton_Click(object sender, EventArgs e)
        {
            this._serverConfig.SaveChanges();
        }

        /// <summary>
        /// Handles the Click event of the ServerCancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServerCancelButton_Click(object sender, EventArgs e)
        {
            bool canRefresh = true;
            if (this._hasServerPropertyChanges)
            {
                if (MessageBox.Show("There are unsaved changes. Cancel?", "Cancel changes", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    canRefresh = false;
                }
            }

            if (canRefresh)
            {
                this.RefreshServerConfig();
            }
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the ServerPropertyGrid control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        private void ServerPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this._hasServerPropertyChanges = true;
        }

        /// <summary>
        /// Handles the Click event of the ServiceAccountChangeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServiceAccountChangeButton_Click(object sender, EventArgs e)
        {
            // TODO: trivial to move out into a separate class.
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "mmc.exe");
            string args = @"C:\Windows\system32\services.msc";

            Process services = new Process();
            services.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args
            };
            services.Start();

            //AutoItX.Run(string.Format("{0} \"{1}\"", fileName, args), AppDomain.CurrentDomain.BaseDirectory);
            AutoItX.WinWaitActive("Services");

            int result = 0;
            int attempts = 5;

            while (result != 1)
            {
                result = AutoItX.ControlFocus("Services", "Services (Local)", "[CLASS:MMCOCXViewWindow; INSTANCE:1]");
                if (result == 1)
                {
                    AutoItX.ControlSend("Services", "Services (Local)", "[CLASS:SysHeader32; INSTANCE:1]", "{TAB}");
                    AutoItX.Send("SACS{ENTER}");
                }
                else
                {
                    Thread.Sleep(2000);
                    if (attempts-- == 0)
                    {
                        result = 1; // pretend true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the MouseEnter event of the Tooltip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Tooltip_MouseEnter(object sender, EventArgs e)
        {
            // Would prefer to read these off a property on each control, but I'm not phased enough to change it.
            if (sender == ServiceAccountLabel || sender == ServiceAccountTextBox)
            {
                this.TooltipLabel.Text = "Use a service account that is granted admin rights on the machine and can access the SQL database that will have SACS. " +
                    "LOCALSYSTEM cannot access remote SQL instances. LOCALSERVICE has too limited rights on the machine.";
            }
            else if (sender == ServiceAccountChangeButton)
            {
                this.TooltipLabel.Text = "Will attempt to open up the properties box for the service. " +
                    "Otherwise it will just open the services control panel. search for SACS - Windows Service Agent.";
            }
            else if (sender == RefreshAccountButton)
            {
                this.TooltipLabel.Text = "Reloads any changes to the service account.";
            }
            else if (sender == DatabaseLabel)
            {
                this.TooltipLabel.Text = "A database file comes with SACS, but it will only work if SQL server exists on this machine. " +
                    "Otherwise, configure the database on a separate server (make sure the service account has access to it).";
            }
            else if (sender == DatabaseLocationTextBox)
            {
                this.TooltipLabel.Text = "Displays the current location of the SACS database.";
            }
            else if (sender == DeployScriptsButton)
            {
                this.TooltipLabel.Text = "Click to start the SQL script deployment.";
            }
            else if (sender == ServerApplyButton)
            {
                this.TooltipLabel.Text = "Will save the current changes to SACS.WindowsService.exe.config file.";
            }
            else if (sender == ServerCancelButton)
            {
                this.TooltipLabel.Text = "Will discard current changes and reload the SACS.WindowsService.exe.config file.";
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the Tooltip control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Tooltip_MouseLeave(object sender, EventArgs e)
        {
            this.TooltipLabel.Text = string.Empty;
        }

        #endregion Event Handlers

        #region Methods

        private void LoadWindowsConsoleConfig()
        {
        }

        private void SaveWindowsConsoleConfig()
        {
        }

        /// <summary>
        /// Loads the server configuration.
        /// </summary>
        private void LoadServerConfig()
        {
            if (this._serverConfig.HasChanges)
            {
                MessageBox.Show("There are unsaved changes to the server's app.config properties. Either save or cancel the changes.");
            }
            else
            {
                this.RefreshServerConfig();
            }
        }

        /// <summary>
        /// Refreshes the server configuration.
        /// </summary>
        private void RefreshServerConfig()
        {
            this.ServerApplyButton.Enabled = this.ServerCancelButton.Enabled = false;

            this._serverConfig.RefreshFromFile(Path.Combine(InstallationManager.Current.CurrentServerLocation, "SACS.WindowsService.exe"));
            this.ServerPropertyGrid.SelectedObject = this._serverConfig;
            this.DatabaseLocationTextBox.Text = this._serverConfig.DatabaseLocation;

            this.ServerApplyButton.Enabled = this.ServerCancelButton.Enabled = true;
            this._hasServerPropertyChanges = false;
        }

        /// <summary>
        /// Refreshes the server login.
        /// </summary>
        private void RefreshServerLogin()
        {
            var invalidAccounts = new string[] { "LocalSystem" };
            this.ServiceAccountTextBox.Text = InstallationManager.Current.ServiceAccount;

            if (string.IsNullOrWhiteSpace(this.ServiceAccountTextBox.Text) ||
                invalidAccounts.Contains(this.ServiceAccountTextBox.Text))
            {
                this.ServerErrorProvider.SetError(this.ServiceAccountLabel, "Use a different service account");
            }

            this.ServiceAccountChangeButton.Enabled = !string.IsNullOrWhiteSpace(InstallationManager.Current.ServiceAccount);
        }

        /// <summary>
        /// Saves the server configuration.
        /// </summary>
        private void SaveServerConfig()
        {
        }

        #endregion Methods
    }
}