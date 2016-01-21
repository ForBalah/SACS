using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SACS.Setup.Classes;
using SACS.Setup.Forms;
using SACS.Setup.Log;

namespace SACS.Setup
{
    public partial class SetupForm : Form
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupForm"/> class.
        /// </summary>
        public SetupForm()
        {
            InitializeComponent();

            WizardManager.Current = new WizardManager(this, this.MainNavigationButtons, new InstallProgressForm());
            WizardManager.Current.AddTab(
                "Welcome",
                new WizardTabSettings
                {
                    TabPage = this.WelcomeTabPage,
                    Label = this.WelcomeLabel,
                    Position = Setup.Controls.NavigationPosition.Start,
                    State = SetupState.InProgress,
                    NextTab = "Installation Type"
                });
            WizardManager.Current.AddTab(
                "Installation Type",
                new WizardTabSettings
                {
                    TabPage = this.InstallationTabPage,
                    Label = this.InstallationTypeLabel,
                    Position = Setup.Controls.NavigationPosition.Middle,
                    State = SetupState.InProgress,
                    PreviousTab = "Welcome",
                    NextTab = "Checklist"
                });
            WizardManager.Current.AddTab(
                "Uninstall",
                new WizardTabSettings
                {
                    TabPage = this.UninstallTabPage,
                    Label = this.UninstallLabel,
                    Position = Setup.Controls.NavigationPosition.Middle,
                    State = SetupState.InProgress,
                    PreviousTab = "Installation Type",
                    NextTab = "Complete"
                });
            WizardManager.Current.AddTab(
                "Install / Upgrade",
                new WizardTabSettings
                {
                    TabPage = this.PerformInstallTabPage,
                    Label = this.InstallUpgradeLabel,
                    Position = Setup.Controls.NavigationPosition.Middle,
                    State = SetupState.InProgress,
                    PreviousTab = "Installation Type",
                    NextTab = "Checklist"
                });
            WizardManager.Current.AddTab(
                "Configure",
                new WizardTabSettings
                {
                    TabPage = this.ConfigureTabPage,
                    Label = this.ConfigureLabel,
                    Position = Setup.Controls.NavigationPosition.Middle,
                    State = SetupState.InProgress,
                    PreviousTab = "Install / Upgrade",
                    NextTab = "Checklist"
                });
            WizardManager.Current.AddTab(
                "Checklist",
                new WizardTabSettings
                {
                    TabPage = this.ChecklistTabPage,
                    Label = this.ChecklistLabel,
                    Position = Setup.Controls.NavigationPosition.Middle,
                    State = SetupState.InProgress,
                    PreviousTab = "Configure",
                    NextTab = "Complete"
                });
            WizardManager.Current.AddTab(
                "Complete",
                new WizardTabSettings
                {
                    TabPage = this.CompleteTabPage,
                    Label = this.CompleteLabel,
                    Position = Setup.Controls.NavigationPosition.Finish,
                    State = SetupState.Complete,
                    PreviousTab = "Checklist"
                });
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the form is in design mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the form is in design mode; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// This value is determined by the usage mode as well as the overriding debug
        /// "code" which can be specified in the app.config.
        /// </para>
        /// Under normal circumstances, this would have been a property in a separate
        /// class, but since this will be one window, it exists here.
        /// </remarks>
        public bool InDesignMode
        {
            get
            {
                bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

                // I know this looks arbitrary, but I would much prefer not giving the simple option
                // of setting the debug mode without looking at the code, disassembling the installer
                // or finding it on the internet.
                bool debugMode = ConfigurationManager.AppSettings["DebugCode"] == "438010367";

                return designMode || debugMode;
            }
        }

        /// <summary>
        /// Gets the tab control.
        /// </summary>
        /// <value>
        /// The tab control.
        /// </value>
        public TabControl TabControl
        {
            get
            {
                return this.MainSetupTabControl;
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            LogHelper.GetLogger(this.GetType()).Log("CurrentDomain_UnhandledException", exception);
            this.ShowError(exception);
            LogHelper.TryOpenLog();
        }

        /// <summary>
        /// Handles the UIThreadException event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ThreadExceptionEventArgs"/> instance containing the event data.</param>
        public void Application_UIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogHelper.GetLogger(this.GetType()).Log("Application_UIThreadException", e.Exception);
            this.ShowError(e.Exception);
            LogHelper.TryOpenLog();
        }

        /// <summary>
        /// Handles the Cancel event of the MainNavigationButtons control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainNavigationButtons_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Selected event of the MainSetupTabControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TabControlEventArgs"/> instance containing the event data.</param>
        private void MainSetupTabControl_Selected(object sender, TabControlEventArgs e)
        {
            // mainly for debug purposes to make sure the tabs are always in sync with the manager
            if (e.Action == TabControlAction.Selected && !WizardManager.Current.IsCurrentTab(e.TabPage))
            {
                WizardManager.Current.FindAndSelectTabPage(e.TabPage);
            }
        }

        /// <summary>
        /// Handles the Selecting event of the MainSetupTabControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TabControlCancelEventArgs"/> instance containing the event data.</param>
        private void MainSetupTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!this.InDesignMode)
            {
                // prevent movement off the existing tab.
                if (!WizardManager.Current.IsCurrentTab(e.TabPage))
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the SetupForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void SetupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !WizardManager.Current.TryClose();
        }

        /// <summary>
        /// Handles the Load event of the SetupForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SetupForm_Load(object sender, EventArgs e)
        {
            LogHelper.GetLogger(this.GetType()).Log("===== Starting Setup =====");
            this.Text += " - version " + this.ProductVersion;
            this.RepositionControls();
            WizardManager.Current.SelectTab("Welcome");
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="e">The e.</param>
        public void ShowError(Exception e)
        {
            MessageBox.Show(e.Message, "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Repositions the controls if not in debug mode
        /// </summary>
        private void RepositionControls()
        {
            if (!this.InDesignMode)
            {
                int oldTop = MainSetupTabControl.Top;
                int newTop = 29;
                MainSetupTabControl.Top = 29;
                MainSetupTabControl.Height += (oldTop - newTop);
            }
        }

        #endregion Methods
    }
}