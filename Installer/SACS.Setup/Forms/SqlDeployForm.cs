using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SACS.Setup.Classes;

namespace SACS.Setup.Forms
{
    /// <summary>
    /// The SQL deployment form
    /// </summary>
    public partial class SqlDeployForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDeployForm"/> class.
        /// </summary>
        public SqlDeployForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the CopyButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(this.ScriptsTextBox.Text);
            MessageBox.Show("Contents copied to clipboard");
        }

        /// <summary>
        /// Handles the Load event of the SqlDeployForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SqlDeployForm_Load(object sender, EventArgs e)
        {
            string codeBase = Assembly.GetAssembly(typeof(SqlDeployForm)).CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            this.ScriptsTextBox.Text = File.ReadAllText(FileSystemUtilities.DeploymentScriptTempPath);
        }
    }
}