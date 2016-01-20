using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;
using SACS.Setup.Classes;

namespace SACS.Setup.Controls
{
    /// <summary>
    /// The configure SACS setup control
    /// </summary>
    public partial class ConfigureControl : UserControl
    {
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
            }
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

            AutoItX.Run(string.Format("{0} \"{1}\"", fileName, args), AppDomain.CurrentDomain.BaseDirectory);
            AutoItX.WinWaitActive("Services");
            int result = AutoItX.ControlFocus("Services", "Services (Local)", "[CLASS:MMCOCXViewWindow; INSTANCE:1]");

            if (result == 1)
            {
                AutoItX.ControlSend("Services", "Services (Local)", "[CLASS:SysHeader32; INSTANCE:1]", "{TAB}");
                AutoItX.Send("SACS{ENTER}");
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
            if (sender == ServiceAccountLabel || sender == ServiceAccountTextBox || sender == ServiceAccountChangeButton)
            {
                this.TooltipLabel.Text = "Use a service account that has admin rights on the machine and can access the SQL database that will have SACS. " +
                    "LOCALSYSTEM cannot access remote SQL instances. LOCALSERVICE is that it has limited rights on the machine.";
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