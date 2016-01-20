using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SACS.Setup.Controls;

namespace SACS.Setup.Classes
{
    /// <summary>
    /// Represents a tab in the wizard
    /// </summary>
    public class WizardTab
    {
        public WizardTab(string name, WizardTabSettings settings)
        {
            this.Name = name;
            this.TabPage = settings.TabPage;
            this.Label = settings.Label;
            this.Position = settings.Position;
            this.State = settings.State;
            this.NextTab = settings.NextTab;
            this.PreviousTab = settings.PreviousTab;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public Label Label { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the tab page.
        /// </summary>
        /// <value>
        /// The tab page.
        /// </value>
        public TabPage TabPage { get; private set; }

        /// <summary>
        /// Gets sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public NavigationPosition Position { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public SetupState State { get; private set; }

        /// <summary>
        /// Gets or sets the next tab.
        /// </summary>
        /// <value>
        /// The next tab.
        /// </value>
        public string NextTab { get; set; }

        /// <summary>
        /// Gets or sets the previous tab.
        /// </summary>
        /// <value>
        /// The previous tab.
        /// </value>
        public string PreviousTab { get; set; }
    }
}