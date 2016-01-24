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
    /// <summary>
    /// Contains the back, next, cancel, and finish buttons.
    /// </summary>
    public partial class NavigationButtons : UserControl
    {
        private NavigationPosition _Position;

        /// <summary>
        /// Occurs when the cancel/finish button is clicked.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        /// Occurs when the next button is clicked.
        /// </summary>
        public event CancelEventHandler Next;

        /// <summary>
        /// Occurs when the back button is clicked.
        /// </summary>
        public event CancelEventHandler Back;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationButtons"/> class.
        /// </summary>
        public NavigationButtons()
        {
            InitializeComponent();
            this.UpdateButtons();
        }

        public bool CanGoNext
        {
            get
            {
                return this.NextButton.Enabled;
            }

            set
            {
                this.NextButton.Enabled = value && this.Position != NavigationPosition.Finish;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public NavigationPosition Position
        {
            get
            {
                return this._Position;
            }

            set
            {
                this._Position = value;
                this.UpdateButtons();
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelWizardButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CancelWizardButton_Click(object sender, EventArgs e)
        {
            if (this.Cancel != null)
            {
                this.Cancel(this, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the NextButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NextButton_Click(object sender, EventArgs e)
        {
            var eventArgs = new CancelEventArgs();
            if (this.Next != null)
            {
                this.Next(this, eventArgs);
            }

            if (!eventArgs.Cancel)
            {
                WizardManager.Current.GoNext();
            }
        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            var eventArgs = new CancelEventArgs();
            if (this.Back != null)
            {
                this.Back(this, eventArgs);
            }

            if (!eventArgs.Cancel)
            {
                WizardManager.Current.GoBack();
            }
        }

        /// <summary>
        /// Updates the buttons.
        /// </summary>
        private void UpdateButtons()
        {
            switch (this._Position)
            {
                case NavigationPosition.Start:
                    this.BackButton.Visible = false;
                    this.CancelWizardButton.Text = "Cancel";
                    this.CanGoNext = true;
                    break;

                case NavigationPosition.Middle:
                    this.BackButton.Visible = true;
                    this.BackButton.Enabled = true;
                    this.CancelWizardButton.Text = "Cancel";
                    this.CanGoNext = true;
                    break;

                case NavigationPosition.Finish:
                    this.BackButton.Visible = true;
                    this.BackButton.Enabled = true;
                    this.CancelWizardButton.Text = "Finish";
                    this.CanGoNext = false;
                    break;

                default:
                    throw new NotImplementedException("'position' in UpdateButtons not implemented.");
            }
        }
    }

    /// <summary>
    /// Represents the position of the navigation buttons.
    /// </summary>
    public enum NavigationPosition
    {
        /// <summary>
        /// At the start
        /// </summary>
        Start = 0,

        /// <summary>
        /// In the middle
        /// </summary>
        Middle,

        /// <summary>
        /// At the finish
        /// </summary>
        Finish
    }
}