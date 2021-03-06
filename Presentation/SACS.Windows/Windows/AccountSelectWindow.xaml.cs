﻿using System.Windows;
using SACS.BusinessLayer.Extensions;

namespace SACS.Windows.Windows
{
    /// <summary>
    /// Interaction logic for AccountSelectWindow.xaml
    /// </summary>
    public partial class AccountSelectWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountSelectWindow"/> class.
        /// </summary>
        public AccountSelectWindow()
        {
            this.InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this is a custom account selection.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this is a custom account selection; otherwise, <c>false</c>.
        /// </value>
        public bool IsCustomAccount
        {
            get
            {
                return this.CustomAccountRadioButton.IsChecked == true;
            }

            set
            {
                this.CustomAccountRadioButton.IsChecked = value;
                this.BuiltInAccountRadioButton.IsChecked = !value;
                this.UpdateDisplay();
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return this.UsernameTextBox.Text;
            }

            set
            {
                this.UsernameTextBox.Text = value;
            }
        }

        /// <summary>
        /// Gets the encrypted password.
        /// </summary>
        /// <value>
        /// The encrypted password.
        /// </value>
        public string EncryptedPassword
        {
            get
            {
                return this.PasswordTextBox.SecurePassword.EncryptString(EntropyValue);
            }
        }

        /// <summary>
        /// Gets or sets the entropy value
        /// </summary>
        public string EntropyValue
        {
            get; set;
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.UsernameTextBox.Text = this.UsernameTextBox.Text.Trim();
            bool isValid = true;

            if (this.IsCustomAccount)
            {
                if (string.IsNullOrWhiteSpace(this.UsernameTextBox.Text))
                {
                    isValid = false;
                    MessageBox.Show("Please enter a user name.", "Validation Error");
                }
                else if (this.PasswordTextBox.Password != this.ConfirmPasswordTextBox.Password)
                {
                    isValid = false;
                    MessageBox.Show("Passwords do not match.", "Validation Error");
                }
            }

            if (isValid)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the Checked event of the RadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsLoaded)
            {
                this.UpdateDisplay();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateDisplay();
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Updates the display.
        /// </summary>
        private void UpdateDisplay()
        {
            this.UsernameTextBox.IsEnabled = this.IsCustomAccount;
            this.PasswordTextBox.IsEnabled = this.IsCustomAccount;
            this.ConfirmPasswordTextBox.IsEnabled = this.IsCustomAccount;
        }

        #endregion Methods
    }
}