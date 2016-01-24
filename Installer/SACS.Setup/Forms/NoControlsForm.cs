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
    /// <summary>
    /// Represents a simple form with no controls on it.
    /// </summary>
    public partial class NoControlsForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoControlsForm"/> class.
        /// </summary>
        public NoControlsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.MessageLabel.Text; }
            set { this.MessageLabel.Text = value; }
        }
    }
}