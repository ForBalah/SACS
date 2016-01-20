using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SACS.Setup.Forms
{
    public partial class InstallProgressForm : Form
    {
        public InstallProgressForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the InstallProgressForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void InstallProgressForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Sets the caption.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetCaption(string text)
        {
            this.CaptionLabel.Text = text;
        }

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetProgress(int value)
        {
            value = value < 0 ? 0 : value > 100 ? 100 : value;
            this.InstallProgressBar.Value = value;
        }
    }
}