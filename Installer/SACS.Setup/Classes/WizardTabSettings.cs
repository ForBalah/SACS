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
    /// Contains the settings for creating a new wizard tab
    /// </summary>
    public class WizardTabSettings
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public Label Label { get; set; }

        /// <summary>
        /// Gets or sets the tab page.
        /// </summary>
        /// <value>
        /// The tab page.
        /// </value>
        public TabPage TabPage { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public NavigationPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public SetupState State { get; set; }

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