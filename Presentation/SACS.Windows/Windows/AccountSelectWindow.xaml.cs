using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SACS.BusinessLayer.BusinessLogic.Security;
using SACS.BusinessLayer.Extensions;
using SACS.Common.Configuration;

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
            InitializeComponent();
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
        /// Gets or sets the encrypted password.
        /// </summary>
        /// <value>
        /// The encrypted password.
        /// </value>
        public SecureString Password
        {
            get
            {
                return this.PasswordTextBox.SecurePassword;
            }

            set
            {
                this.PasswordTextBox.Password = value.ToManagedString();
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
                if (!string.IsNullOrWhiteSpace(this.PasswordTextBox.Password))
                {
                    return StringCipher.Encrypt(this.PasswordTextBox.Password, ApplicationSettings.Current.EncryptionSecretKey);
                }

                return null;
            }
        }

        #endregion

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

        #endregion

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

        #endregion
    }
}
