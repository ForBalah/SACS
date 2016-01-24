using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SACS.Setup.Controls;
using SACS.Setup.Forms;
using SACS.Setup.Log;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// Main logic for the wizard
    /// </summary>
    public class WizardManager
    {
        #region Fields

        private SetupForm _form;
        private List<WizardTab> _WizardTabs = new List<WizardTab>();
        private NavigationButtons _navigationButtons;
        private InstallProgressForm _progressForm;

        #endregion Fields

        #region Constructors

        public WizardManager(SetupForm form, NavigationButtons navigationButtons, InstallProgressForm progressForm)
        {
            this._form = form;
            this._navigationButtons = navigationButtons;
            this._navigationButtons.Position = NavigationPosition.Start;
            this._progressForm = progressForm;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the current wizard manager.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static WizardManager Current { get; set; }

        /// <summary>
        /// Gets the current setup state.
        /// </summary>
        /// <value>
        /// The current setup state.
        /// </value>
        public SetupState CurrentState
        {
            get
            {
                if (this.CurrentTab != null)
                {
                    return this.CurrentTab.State;
                }

                return SetupState.Unknown;
            }
        }

        /// <summary>
        /// Gets or sets the current wizard tab.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public WizardTab CurrentTab
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <value>
        /// The tabs.
        /// </value>
        public IReadOnlyList<WizardTab> Tabs
        {
            get
            {
                return this._WizardTabs.AsReadOnly();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds the tab to the manager.
        /// </summary>
        /// <param name="wizardTab">The wizard tab.</param>
        public void AddTab(WizardTab wizardTab)
        {
            Debug.Assert(wizardTab != null);

            if (!this._WizardTabs.Any(t => t.Name.Equals(wizardTab.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                this._WizardTabs.Add(wizardTab);
            }
        }

        /// <summary>
        /// Adds the tab to the manager.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="settings">The settings.</param>
        public void AddTab(string name, WizardTabSettings settings)
        {
            this.AddTab(new WizardTab(name, settings));
        }

        /// <summary>
        /// Enables the next navigation.
        /// </summary>
        public void AllowNextNavigation()
        {
            this._navigationButtons.CanGoNext = true;
        }

        /// <summary>
        /// Finds the wizard tab from the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public WizardTab FindTab(string name)
        {
            return this._WizardTabs.FirstOrDefault(t => t.Name.Equals(name));
        }

        /// <summary>
        /// Finds and selects wizard tab page.
        /// </summary>
        /// <param name="tabPage">The tab page.</param>
        public void FindAndSelectTabPage(TabPage tabPage)
        {
            var tab = this._WizardTabs.FirstOrDefault(t => t.TabPage == tabPage);
            string name = tab != null ? tab.Name : null;

            this.SelectTab(name);
        }

        /// <summary>
        /// Goes to the next tab.
        /// </summary>
        public void GoNext()
        {
            if (this.CurrentTab != null && !string.IsNullOrEmpty(this.CurrentTab.NextTab))
            {
                this.SelectTab(this.CurrentTab.NextTab);
            }
        }

        /// <summary>
        /// Goes to the previous tab.
        /// </summary>
        public void GoBack()
        {
            if (this.CurrentTab != null && !string.IsNullOrEmpty(this.CurrentTab.PreviousTab))
            {
                this.SelectTab(this.CurrentTab.PreviousTab);
            }
        }

        /// <summary>
        /// Hides the progress dialog.
        /// </summary>
        public void HideProgressDialog()
        {
            this.PerformOnUI(() =>
                {
                    // NOTE order is very important: enabled must come before hide.
                    this._form.Enabled = true;
                    this._progressForm.Hide();
                });
        }

        /// <summary>
        /// Determines whether the specified tab page is the current open tab.
        /// </summary>
        /// <param name="tabPage">The tab page.</param>
        /// <returns></returns>
        public bool IsCurrentTab(TabPage tabPage)
        {
            if (this.CurrentTab != null)
            {
                return this.CurrentTab.TabPage == tabPage;
            }

            return false;
        }

        /// <summary>
        /// Performs the on UI.
        /// </summary>
        /// <param name="action">The action.</param>
        public void PerformOnUI(Action action)
        {
            this._form.Invoke(action);
        }

        /// <summary>
        /// Disables the next navigation.
        /// </summary>
        public void PreventNextNavigation()
        {
            this._navigationButtons.CanGoNext = false;
        }

        /// <summary>
        /// Selects the wizard tab based on name.
        /// </summary>
        /// <param name="name">The wizard tab name.</param>
        public void SelectTab(string name)
        {
            foreach (var wizardTab in this._WizardTabs)
            {
                if (wizardTab.Name == name)
                {
                    this.CurrentTab = wizardTab;
                    this._navigationButtons.Position = wizardTab.Position; // this needs to go before SelectedTab
                    this._form.TabControl.SelectedTab = wizardTab.TabPage;
                    wizardTab.Label.BackColor = Color.FromArgb(54, 127, 169);
                    wizardTab.Label.ForeColor = Color.White;
                    wizardTab.Label.Font = new Font(wizardTab.Label.Font, FontStyle.Bold);
                }
                else
                {
                    wizardTab.Label.BackColor = Color.FromArgb(192, 225, 244);
                    wizardTab.Label.ForeColor = Color.DarkGray;
                    wizardTab.Label.Font = new Font(wizardTab.Label.Font, FontStyle.Regular);
                }
            }
        }

        /// <summary>
        /// Shows the progress dialog.
        /// </summary>
        public void ShowProgressDialog()
        {
            this.PerformOnUI(() =>
                {
                    // can't use ShowDialog here
                    this._progressForm.Show(this._form);
                    this._form.Enabled = false;
                });
        }

        /// <summary>
        /// Tries to close the the manager (and subsequently the wizard).
        /// </summary>
        /// <returns><c>true</c> if the close is finalized, otherwise, <c>false</c>.</returns>
        public bool TryClose()
        {
            if (this.CurrentState != SetupState.Complete)
            {
                var result = MessageBox.Show(
                    "Cancelling will halt the installation in its' current state. Exit anyway?",
                    "Exit setup",
                    MessageBoxButtons.OKCancel);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            LogHelper.GetLogger(this.GetType()).Log("===== Closing Setup =====");
            return true;
        }

        /// <summary>
        /// Updates the tab navigation.
        /// </summary>
        /// <param name="previousTabName">Name of the previous tab.</param>
        /// <param name="nextTabName">Name of the next tab.</param>
        public void UpdateTabNavigation(string previousTabName, string nextTabName)
        {
            var previousTab = this._WizardTabs.FirstOrDefault(t => t.Name == previousTabName);
            var nextTab = this._WizardTabs.FirstOrDefault(t => t.Name == nextTabName);

            if (previousTab != null && nextTab != null)
            {
                nextTab.PreviousTab = previousTabName;
                previousTab.NextTab = nextTabName;
            }
        }

        /// <summary>
        /// Updates the progress text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void UpdateProgressText(string text)
        {
            if (this._progressForm.Visible)
            {
                this.PerformOnUI(() => this._progressForm.SetCaption(text));
            }
        }

        /// <summary>
        /// Updates the progress value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void UpdateProgressValue(decimal value)
        {
            if (this._progressForm.Visible)
            {
                this.PerformOnUI(() => this._progressForm.SetProgress((int)(value * 100)));
            }
        }

        #endregion Methods
    }
}