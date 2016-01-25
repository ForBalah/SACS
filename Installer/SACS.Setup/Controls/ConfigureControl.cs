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
    public partial class ConfigureControl : UserControl, INavigationValidator
    {
        #region Fields

        private bool _HasServerPropertyChanges = false;
        private bool _HasWindowsPropertyChanges = false;
        private ServerConfigFile _serverConfig = new ServerConfigFile();
        private WindowsConsoleConfigFile _windowsConfig = new WindowsConsoleConfigFile();

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

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether there are  server property changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if there are  server property changes; otherwise, <c>false</c>.
        /// </value>
        protected bool HasServerPropertyChanges
        {
            get
            {
                return this._HasServerPropertyChanges;
            }

            set
            {
                this._HasServerPropertyChanges = value;
                this.ServerApplyButton.Font = new Font(this.ServerApplyButton.Font, value ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there are Windows console property changes.
        /// </summary>
        /// <value>
        /// <c>true</c> if there are Windows console property changes; otherwise, <c>false</c>.
        /// </value>
        protected bool HasWindowsPropertyChanges
        {
            get
            {
                return this._HasWindowsPropertyChanges;
            }

            set
            {
                this._HasWindowsPropertyChanges = value;
                this.WindowsApplyButton.Font = new Font(this.WindowsApplyButton.Font, value ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        #endregion Properties

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
                // todo: this should be a singleton instead
                var loadingForm = new NoControlsForm();
                loadingForm.Show(this.Parent);
                Application.DoEvents();
                this.RefreshServerLogin();
                this.LoadServerConfig();
                this.LoadWindowsConsoleConfig();
                loadingForm.Close();
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
            if (this.HasServerPropertyChanges)
            {
                this._serverConfig.SaveChanges();
                this.HasServerPropertyChanges = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the ServerCancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServerCancelButton_Click(object sender, EventArgs e)
        {
            bool canRefresh = true;
            if (this.HasServerPropertyChanges)
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
            this.HasServerPropertyChanges = true;
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
                    "Otherwise it will just open the services control panel. Search for 'SACS - Windows Service Agent'.";
            }
            else if (sender == RefreshAccountButton)
            {
                this.TooltipLabel.Text = "Reloads any changes to the service account.";
            }
            else if (sender == DatabaseLabel)
            {
                this.TooltipLabel.Text = "A database file comes with SACS, but it will only work if SQL server 2012+ exists on this machine. " +
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
            else if (sender == ServerPropertyGrid)
            {
                this.TooltipLabel.Text = "Use this to update key config settings in the server's deployed app.config file.";
            }
            else if (sender == WindowsApplyButton)
            {
                this.TooltipLabel.Text = "Will save the current changes to SACS.Windows.exe.config file.";
            }
            else if (sender == WindowsCancelButton)
            {
                this.TooltipLabel.Text = "Will discard current changes and reload the SACS.Windows.exe.config file.";
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

        /// <summary>
        /// Handles the Click event of the WindowsApplyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WindowsApplyButton_Click(object sender, EventArgs e)
        {
            if (this.HasWindowsPropertyChanges)
            {
                this._windowsConfig.SaveChanges();
                this.HasWindowsPropertyChanges = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the WindowsCancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WindowsCancelButton_Click(object sender, EventArgs e)
        {
            bool canRefresh = true;
            if (this.HasWindowsPropertyChanges)
            {
                if (MessageBox.Show("There are unsaved changes. Cancel?", "Cancel changes", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    canRefresh = false;
                }
            }

            if (canRefresh)
            {
                this.RefreshWindowsConsoleConfig();
            }
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the WindowsPropertyGrid control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        private void WindowsPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.HasWindowsPropertyChanges = true;
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Validates the ability to go next.
        /// </summary>
        /// <returns></returns>
        public bool ValidateGoNext()
        {
            if (this.HasServerPropertyChanges)
            {
                MessageBox.Show("There are unsaved server config changes. Please apply or cancel them before continuing.");
                this.ConfigureTabControl.SelectedTab = ServerTabPage;
                return false;
            }
            if (this.HasWindowsPropertyChanges)
            {
                MessageBox.Show("There are unsaved Windows console changes. Please apply or cancel them before continuing.");
                this.ConfigureTabControl.SelectedTab = WindowsTabPage;
                return false;
            }

            return true;
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
        /// Loads the windows console configuration.
        /// </summary>
        private void LoadWindowsConsoleConfig()
        {
            if (this._windowsConfig.HasChanges)
            {
                MessageBox.Show("There are unsaved changes to the Windows console's app.config properties. Either save or cancel the changes.");
            }
            else
            {
                this.RefreshWindowsConsoleConfig();
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
            this.HasServerPropertyChanges = false;
        }

        /// <summary>
        /// Refreshes the windows console configuration.
        /// </summary>
        private void RefreshWindowsConsoleConfig()
        {
            this.WindowsApplyButton.Enabled = this.WindowsCancelButton.Enabled = false;

            this._windowsConfig.RefreshFromFile(Path.Combine(InstallationManager.Current.CurrentWindowsConsoleLocation, "SACS.Windows.exe"));
            this.WindowsPropertyGrid.SelectedObject = this._windowsConfig;

            this.WindowsApplyButton.Enabled = this.WindowsCancelButton.Enabled = true;
            this.HasWindowsPropertyChanges = false;
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

        #endregion Methods
    }
}