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
    public partial class InstallationTypeControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallationTypeControl"/> class.
        /// </summary>
        public InstallationTypeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the MouseLeave event of the RadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RadioButton_MouseLeave(object sender, EventArgs e)
        {
            this.TooltipLabel.Text = string.Empty;
        }

        /// <summary>
        /// Handles the MouseEnter event of the InstallRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void InstallRadioButton_MouseEnter(object sender, EventArgs e)
        {
            this.TooltipLabel.Text = "Use this option to perform a fresh installation or to upgrade to the version included in this wizard." +
                " Configuring the installation is included with this option.";
        }

        /// <summary>
        /// Handles the MouseEnter event of the ConfigureRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ConfigureRadioButton_MouseEnter(object sender, EventArgs e)
        {
            this.TooltipLabel.Text = "Use this option to configure an existing SACS installation on this system.";
        }

        /// <summary>
        /// Handles the MouseEnter event of the UninstallRadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UninstallRadioButton_MouseEnter(object sender, EventArgs e)
        {
            this.TooltipLabel.Text = "Use this option to remove an existing SACS installation from the system. This will not remove Service Apps that the server contains.";
        }

        /// <summary>
        /// Handles the VisibleChanged event of the InstallationTypeControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void InstallationTypeControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.SetupNavigation();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the RadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.SetupNavigation();

            if (WizardManager.Current != null)
            {
                if (this.InstallRadioButton.Checked)
                {
                    WizardManager.Current.UpdateTabNavigation("Installation Type", "Install / Upgrade");
                    WizardManager.Current.UpdateTabNavigation("Welcome", "Install / Upgrade");
                    WizardManager.Current.UpdateTabNavigation("Install / Upgrade", "Configure");
                }
                else if (this.ConfigureRadioButton.Checked)
                {
                    WizardManager.Current.UpdateTabNavigation("Installation Type", "Configure");
                    WizardManager.Current.UpdateTabNavigation("Welcome", "Configure");
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
                if (InstallationGroupBox.Controls.OfType<RadioButton>().Any(r => r.Checked))
                {
                    WizardManager.Current.AllowNextNavigation();
                }
                else
                {
                    WizardManager.Current.PreventNextNavigation();
                }
            }
        }
    }
}