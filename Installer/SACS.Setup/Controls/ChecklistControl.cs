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
    public partial class ChecklistControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecklistControl"/> class.
        /// </summary>
        public ChecklistControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the ChecklistControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ChecklistControl_Load(object sender, EventArgs e)
        {
            this.SetupNavigation();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WizardManager.Current.AllowNextNavigation();
        }

        /// <summary>
        /// Handles the VisibleChanged event of the ChecklistControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ChecklistControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.SetupNavigation();
            }
        }

        /// <summary>
        /// Setups the navigation.
        /// </summary>
        private void SetupNavigation()
        {
            if (WizardManager.Current != null)
            {
                WizardManager.Current.PreventNextNavigation();
                // then check the state of the control before allowing next
            }
        }
    }
}