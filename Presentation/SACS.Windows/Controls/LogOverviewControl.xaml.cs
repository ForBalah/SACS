using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories;
using SACS.Windows.ViewModels;
using SACS.Windows.Windows;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for LogsControl.xaml
    /// </summary>
    public partial class LogsControl : UserControl, ILogOverviewView
    {
        private readonly LogOverviewPresenter _presenter;
        private List<string> _currentLogs = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsControl"/> class.
        /// </summary>
        public LogsControl()
        {
            this.InitializeComponent();
            this._presenter = new LogOverviewPresenter(this, new WebApiClientFactory());
            this.LogsNameListBox.ItemsSource = this._currentLogs;
        }

        #region Event Handlers

        /// <summary>
        /// Handles the MouseDoubleClick event of the LogsNameListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void LogsNameListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.LogsNameListBox.SelectedValue != null)
                {
                    this.OpenLog((string)this.LogsNameListBox.SelectedValue);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the OpenLogButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OpenLogButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.LogsNameListBox.SelectedValue != null)
            {
                this.OpenLog((string)this.LogsNameListBox.SelectedValue);
            }
            else
            {
                MessageBox.Show("Select a log file first");
            }
        }

        /// <summary>
        /// Handles the Click event of the RefreshButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this._presenter.LoadControl();
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._presenter.LoadControl();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the log.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        private void OpenLog(string fileName)
        {
            var logViewer = new LogViewerWindow();
            logViewer.SetFileName(fileName);
            logViewer.ShowDialog();
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            this.LogsErrorLabel.Text = e.Message;
        }

        /// <summary>
        /// Clears the exception.
        /// </summary>
        public void ClearException()
        {
            this.LogsErrorLabel.Text = null;
        }

        /// <summary>
        /// Sets the current logs.
        /// </summary>
        /// <param name="logs">The logs.</param>
        public void SetCurrentLogs(IEnumerable<string> logs)
        {
            if (logs != null)
            {
                this._currentLogs.Clear();
                this._currentLogs.AddRange(logs.Select(l => System.IO.Path.GetFileName(l)));
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(this.LogsNameListBox.ItemsSource);
            view.Refresh();
        }

        #endregion
    }
}
