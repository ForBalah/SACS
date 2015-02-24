using System;
using System.Collections.Generic;
using System.Linq;
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
using SACS.DataAccessLayer.Models;

namespace SACS.Windows.Windows
{
    /// <summary>
    /// Interaction logic for LogFilterWindow.xaml
    /// </summary>
    public partial class LogFilterWindow : Window
    {
        /// <summary>
        /// Gets the entries that were set on the window
        /// </summary>
        /// <value>
        /// The entries.
        /// </value>
        public IEnumerable<LogEntry> Entries { get; private set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName
        {
            get { return this.UserNameTextBox.Text; }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public string Level
        {
            get 
            { 
                if (this.LevelComboBox.SelectedIndex != -1)
                {
                    return this.LevelComboBox.SelectedValue.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get { return this.MessageTextBox.Text; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFilterWindow"/> class.
        /// </summary>
        /// <param name="entries">The entries.</param>
        public LogFilterWindow(IEnumerable<LogEntry> entries)
        {
            this.Entries = entries;
            InitializeComponent();
            this.PopulateLevelDropDown();
        }

        /// <summary>
        /// Populates the level drop down.
        /// </summary>
        private void PopulateLevelDropDown()
        {
            var levels = (from e in Entries select e.Level).Distinct().ToList();
            this.LevelComboBox.ItemsSource = levels;
        }

        /// <summary>
        /// Handles the Click event of the ClearButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.UserNameTextBox.Text = string.Empty;
            this.MessageTextBox.Text = string.Empty;
            this.LevelComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Handles the Click event of the OkButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
