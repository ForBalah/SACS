using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SACS.Setup.Classes;

namespace SACS.Setup.Controls
{
    public partial class InstallUpdateControl : UserControl
    {
        #region Fields

        private bool _ServerInstallCompleted;
        private bool _WindowsInstallCompleted;

        #endregion Fields

        #region Constructors and destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallUpdateControl"/> class.
        /// </summary>
        public InstallUpdateControl()
        {
            InitializeComponent();
        }

        #endregion Constructors and destructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the windows console install part is completed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the windows console install part is completed; otherwise, <c>false</c>.
        /// </value>
        public bool WindowsInstallCompleted
        {
            get
            {
                return this._WindowsInstallCompleted;
            }

            set
            {
                this._WindowsInstallCompleted = value;
                this.SetupNavigation();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the server install part is completed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the server install part is completed; otherwise, <c>false</c>.
        /// </value>
        public bool ServerInstallCompleted
        {
            get
            {
                return this._ServerInstallCompleted;
            }

            set
            {
                this._ServerInstallCompleted = value;
                this.SetupNavigation();
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the ServerBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServerBrowseButton_Click(object sender, EventArgs e)
        {
            this.ServerFolderBrowserDialog.ShowDialog();
            this.ServerLocationTextBox.Text = this.ServerFolderBrowserDialog.SelectedPath;
            this.ServerInstallButton.Enabled = !string.IsNullOrWhiteSpace(this.ServerFolderBrowserDialog.SelectedPath);
        }

        /// <summary>
        /// Handles the Click event of the ServerInstallButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServerInstallButton_Click(object sender, EventArgs e)
        {
            var installer = InstallationManager.Current;
            string message = installer.IsServerUpgrade ? string.Format(
                    "This will update current SACS version (v{0}) to the version contained in this install (v{1}). Proceed?",
                    installer.GetServerVersion(),
                    this.ProductVersion) :
                "This will install the SACS server to the specified location. Proceed?";

            if (!installer.IsServerUpgrade)
            {
                // new installations will need a path set on the installer.
                installer.ServerInstallLocation = this.ServerLocationTextBox.Text;
            }

            if (MessageBox.Show(message, "SACS Installation", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                installer.InstallServer(
                    (success) =>
                    {
                        if (success)
                        {
                            this.ServerInstallCompleted = true;
                            this.EnableGroupBox(this.ServerGroupBox, false);
                            this.ServerCompleteLabel.Visible = true;
                        }
                    });
            }
        }

        /// <summary>
        /// Handles the Click event of the WindowsBrowseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WindowsBrowseButton_Click(object sender, EventArgs e)
        {
            this.WindowsFolderBrowserDialog.ShowDialog();
            this.WindowsLocationTextBox.Text = this.WindowsFolderBrowserDialog.SelectedPath;
            this.WindowsInstallButton.Enabled = !string.IsNullOrWhiteSpace(this.WindowsFolderBrowserDialog.SelectedPath);
        }

        /// <summary>
        /// Handles the Click event of the WindowsInstallButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void WindowsInstallButton_Click(object sender, EventArgs e)
        {
            var installer = InstallationManager.Current;
            string message = installer.IsWindowsConsoleUpgrade ? string.Format(
                    "This will update current SACS Windows Management Console version (v{0}) to the version contained in this install (v{1}). Proceed?",
                    installer.GetWindowsConsoleVersion(),
                    this.ProductVersion) :
                "This will install the SACS Windows Management Console to the specified location. Proceed?";

            if (!installer.IsWindowsConsoleUpgrade)
            {
                // new installations will need a path set on the installer.
                installer.WindowsConsoleInstallLocation = this.WindowsLocationTextBox.Text;
            }

            if (MessageBox.Show(message, "SACS Installation", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                installer.InstallWindowsConsole(
                    (success) =>
                    {
                        if (success)
                        {
                            this.WindowsInstallCompleted = true;
                            this.EnableGroupBox(this.WindowsConsoleGroupBox, false);
                            this.WindowsCompleteLabel.Visible = true;
                        }
                    });
            }
        }

        /// <summary>
        /// Handles the VisibleChanged event of the InstallUpdateControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void InstallUpdateControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.SetupServerInstallation();
                this.SetupWindowsInstallation();
                this.SetupNavigation();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the SeverSkipCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SeverSkipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null)
            {
                this.ServerInstallCompleted = checkbox.Checked;
                this.EnableGroupBox(this.ServerGroupBox, !checkbox.Checked, checkbox);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the WindowsSkipCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void WindowsSkipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox != null)
            {
                this.WindowsInstallCompleted = checkbox.Checked;
                this.EnableGroupBox(this.WindowsConsoleGroupBox, !checkbox.Checked, checkbox, this.WindowsIncludeStartCheckBox);
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
            if (sender == ServerBrowseButton)
            {
                this.TooltipLabel.Text = "Selects the location where the SACS server (the agent and container wrapped in a Windows Service) will be installed to.";
            }
            else if (sender == ServerSkipCheckBox)
            {
                this.TooltipLabel.Text = "Checking this will ignore this server installation section.";
            }
            else if (sender == ServerLocationTextBox)
            {
                this.TooltipLabel.Text = "If this is empty, it means that an existing server is not installed. The browse button will be " +
                    "available to select an installation location.";
            }
            else if (sender == ServerInstallButton)
            {
                this.TooltipLabel.Text = "Performs the installation. If there are no files, the service will be installed here. " +
                    "If SACS is already installed, the files will be backed up and updated. " +
                    "Lastly, if SACS is not installed, but files already exist (with the same version), then the server will just be installed.";
            }
            else if (sender == WindowsBrowseButton)
            {
                this.TooltipLabel.Text = "Selects the location where the Windows Management Console will be installed to.";
            }
            else if (sender == WindowsSkipCheckBox)
            {
                this.TooltipLabel.Text = "Checking this will ignore this Windows Management Console installation section.";
            }
            else if (sender == WindowsLocationTextBox)
            {
                this.TooltipLabel.Text = "If this is empty, it means that an existing Windows Management Console is not installed. The browse button will be " +
                    "available to select an installation location.";
            }
            else if (sender == WindowsInstallButton)
            {
                this.TooltipLabel.Text = "Performs the installation. If there are no files, the console will be installed here. " +
                    "If the console is already installed, the files will be backed up and updated. " +
                    "Lastly, if the console is not installed, but files already exist (with the same version), then the console will just be installed.";
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
        /// Enables the group box.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="controlsToIgnore">The controls to ignore.</param>
        private void EnableGroupBox(GroupBox groupBox, bool enabled, params Control[] controlsToIgnore)
        {
            // TODO: might as well make this into an extension method as this breaks SRP.
            foreach (Control control in groupBox.Controls)
            {
                if (!(controlsToIgnore ?? new Control[0]).Any(c => c == control))
                {
                    control.Enabled = enabled;
                }
            }
        }

        /// <summary>
        /// Setups the navigation buttons based on this control's state.
        /// </summary>
        private void SetupNavigation()
        {
            if (WizardManager.Current != null)
            {
                bool canAllowNavigation = this.ServerInstallCompleted && this.WindowsInstallCompleted;
                if (canAllowNavigation)
                {
                    WizardManager.Current.AllowNextNavigation();
                }
                else
                {
                    WizardManager.Current.PreventNextNavigation();
                }
            }
        }

        /// <summary>
        /// Setups the server installation portion.
        /// </summary>
        private void SetupServerInstallation()
        {
            var installer = InstallationManager.Current;
            var currentPath = installer.CurrentServerLocation;
            var version = installer.GetServerVersion();

            this.ServerLocationTextBox.Text = currentPath;
            this.ServerBrowseButton.Visible = string.IsNullOrWhiteSpace(currentPath);
            this.ServerInstallButton.Enabled = !string.IsNullOrWhiteSpace(currentPath);
            this.ServerInstallButton.Text = string.IsNullOrWhiteSpace(currentPath) ? "Install Now" : "Update Now";
            this.ServerVersionLabel.Text = version != null ? "Existing version: " + version : null;
        }

        /// <summary>
        /// Setups the windows installation portion.
        /// </summary>
        private void SetupWindowsInstallation()
        {
            var installer = InstallationManager.Current;
            var currentPath = installer.CurrentWindowsConsoleLocation;
            var version = installer.GetWindowsConsoleVersion();

            this.WindowsLocationTextBox.Text = currentPath;
            this.WindowsBrowseButton.Visible = string.IsNullOrWhiteSpace(currentPath);
            this.WindowsInstallButton.Enabled = !string.IsNullOrWhiteSpace(currentPath);
            this.WindowsInstallButton.Text = string.IsNullOrWhiteSpace(currentPath) ? "Install Now" : "Update Now";
            this.WindowsVersionLabel.Text = version != null ? "Existing version: " + version : null;
        }

        #endregion Methods
    }
}