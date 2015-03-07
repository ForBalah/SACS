using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Factories;
using SACS.Windows.Extensions;
using SACS.Windows.ViewModels;
using SACS.Windows.Windows;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for LogDetailControl.xaml
    /// </summary>
    public partial class LogDetailControl : UserControl, ILogDetailView
    {
        #region Fields

        private readonly LogDetailPresenter _presenter;
        private string _fileName = string.Empty;

        #endregion

        #region Constructors and Destructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogDetailControl"/> class.
        /// </summary>
        public LogDetailControl()
        {
            this.InitializeComponent();
            this._presenter = new LogDetailPresenter(this, new WebApiClientFactory());
            this.Entries = new List<LogEntryViewModel>();

            this.ErrorImage.Source = ImageExtensions.GetLogImage(LogImageType.Error);
            this.InfoImage.Source = ImageExtensions.GetLogImage(LogImageType.Info);
            this.WarnImage.Source = ImageExtensions.GetLogImage(LogImageType.Warn);
            this.DebugImage.Source = ImageExtensions.GetLogImage(LogImageType.Debug);
        } 

        #endregion

        #region Properties

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <value>
        /// The entries.
        /// </value>
        public List<LogEntryViewModel> Entries
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of the current.
        /// </summary>
        /// <value>
        /// The index of the current.
        /// </value>
        protected int CurrentIndex
        {
            get
            {
                return this.LogListView.SelectedIndex;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the FindNextButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FindNextButton_Click(object sender, RoutedEventArgs e)
        {
            this.FindLogItem(true);
        }

        /// <summary>
        /// Handles the Click event of the FindPreviousButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FindPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this.FindLogItem(false);
        }

        /// <summary>
        /// Handles the KeyDown event of the FindTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void FindTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.FindLogItem(true);
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the LogListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void LogListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Clear();
            LogEntryViewModel logentry = this.LogListView.SelectedItem as LogEntryViewModel;

            if (logentry != null)
            {
                this.LogTypeImage.Source = logentry.Image;
                this.LevelTextBox.Text = logentry.LogEntry.Level;
                this.TimeStampTextBox.Text = string.Format("{0} {1}", logentry.LogEntry.TimeStamp.ToShortDateString(), logentry.LogEntry.TimeStamp.ToShortTimeString());
                this.MachineNameTextBox.Text = logentry.LogEntry.MachineName;
                this.ThreadTextBox.Text = logentry.LogEntry.Thread;
                this.ItemTextBox.Text = logentry.LogEntry.Item.ToString();
                this.HostNameTextBox.Text = logentry.LogEntry.HostName;
                this.UserNameTextBox.Text = logentry.LogEntry.UserName;
                this.AppTextBox.Text = logentry.LogEntry.App;
                this.ClassTextBox.Text = logentry.LogEntry.Class;
                this.MethodTextBox.Text = logentry.LogEntry.Method;
                this.LineTextBox.Text = logentry.LogEntry.Line;
                this.MessageTextBox.Text = logentry.LogEntry.Message;
                this.ThrowableTextBox.Text = logentry.LogEntry.Throwable;
                this.FileTextBox.Text = logentry.LogEntry.File;
            }
        }

        /////// <summary>
        /////// Handles the HeaderClicked event of the LogListView control.
        /////// </summary>
        /////// <param name="sender">The source of the event.</param>
        /////// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        ////private void LogListView_HeaderClicked(object sender, RoutedEventArgs e)
        ////{
        ////    GridViewColumnHeader header = e.OriginalSource as GridViewColumnHeader;
        ////    ListView source = e.Source as ListView;
        ////    try
        ////    {
        ////        ICollectionView dataView = CollectionViewSource.GetDefaultView(source.ItemsSource);
        ////        dataView.SortDescriptions.Clear();
        ////        this._sortDirection = this._sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        ////        SortDescription description = new SortDescription(header.Content.ToString(), this._sortDirection);
        ////        dataView.SortDescriptions.Add(description);
        ////        dataView.Refresh();
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}

        /// <summary>
        /// Handles the Click event of the MenuRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.LoadEntries();
            this.LogListView.SelectedIndex = this.LogListView.Items.Count - 1;
            if (this.LogListView.Items.Count > 4)
            {
                this.LogListView.SelectedIndex -= 3;
            }

            this.LogListView.ScrollIntoView(this.LogListView.SelectedItem);
            ListViewItem lvi = this.LogListView.ItemContainerGenerator.ContainerFromIndex(this.LogListView.SelectedIndex) as ListViewItem;
            if (lvi != null)
            {
                lvi.BringIntoView();
                lvi.Focus();
            }
        }

        /// <summary>
        /// Handles the Click event of the MenuFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuFilter_Click(object sender, RoutedEventArgs e)
        {
            LogFilterWindow filter = new LogFilterWindow(this.Entries.Select(o => o.LogEntry));
            filter.ShowDialog();
            if (filter.DialogResult == true)
            {
                // TODO: move this to presenter
                string level = filter.Level;
                string message = filter.Message;
                string username = filter.UserName;

                IEnumerable<LogEntryViewModel> query = this.Entries;

                if (!string.IsNullOrWhiteSpace(level))
                {
                    query = query.Where(o => o.LogEntry.Level == level);
                }

                if (!string.IsNullOrWhiteSpace(message))
                {
                    query = query.Where(o => o.LogEntry.ContainsText(message));
                }

                if (!string.IsNullOrWhiteSpace(username))
                {
                    query = query.Where(o => (o.LogEntry.UserName ?? string.Empty).ToLower().Contains(username.ToLower()));
                }

                this.LogListView.ItemsSource = query.ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                // TODO: move this to a base class (so that it can be removed from all the other controls)
                MessageBox.Show(string.Format("{0}\r\n\r\nView server logs for more details.", e.Message), title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Sets the name of the file to open
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        public void SetFileName(string logFileName)
        {
            this._fileName = logFileName;
            this.LoadEntries();
        }

        /// <summary>
        /// Loads the log entries.
        /// </summary>
        public void LoadEntries()
        {
            var newEntries = this._presenter.GetEntries(this._fileName).Collection.Select(e => new LogEntryViewModel(e));

            this.Entries.Clear();
            this.LogListView.ItemsSource = null;
            this.Clear();
            this.Entries.AddRange(newEntries);

            this.LogListView.ItemsSource = this.Entries;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        private void Clear()
        {
            this.LevelTextBox.Text = string.Empty;
            this.TimeStampTextBox.Text = string.Empty;
            this.MachineNameTextBox.Text = string.Empty;
            this.ThreadTextBox.Text = string.Empty;
            this.ItemTextBox.Text = string.Empty;
            this.HostNameTextBox.Text = string.Empty;
            this.UserNameTextBox.Text = string.Empty;
            this.AppTextBox.Text = string.Empty;
            this.ClassTextBox.Text = string.Empty;
            this.MethodTextBox.Text = string.Empty;
            this.LineTextBox.Text = string.Empty;
            this.FileTextBox.Text = string.Empty;
            this.MessageTextBox.Text = string.Empty;
            this.ThrowableTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Finds the specified direction.
        /// </summary>
        /// <param name="findNext">If set to <c>true</c> find next, otherwise find previous.</param>
        private void FindLogItem(bool findNext)
        {
            if (this.FindTextBox.Text.Length > 0)
            {
                if (findNext)
                {
                    for (int i = this.CurrentIndex + 1; i < this.LogListView.Items.Count; i++)
                    {
                        bool foundItem = this.Find(i);
                        if (foundItem)
                        {
                            break;
                        }

                        if (i == this.LogListView.Items.Count - 1)
                        {
                            MessageBox.Show("Reached the end of the log.", "Not Found");
                        }
                    }
                }
                else
                {
                    for (int i = this.CurrentIndex - 1; i >= 0 && i < this.LogListView.Items.Count; i--)
                    {
                        bool foundItem = this.Find(i);
                        if (foundItem)
                        {
                            break;
                        }

                        if (i == 1)
                        {
                            MessageBox.Show("Reached the beginning of the log.", "Not Found");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds the log entry at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private bool Find(int index)
        {
            LogEntryViewModel item = (LogEntryViewModel)this.LogListView.Items[index];
            if (item.LogEntry.ContainsText(this.FindTextBox.Text))
            {
                this.LogListView.SelectedIndex = index;
                this.LogListView.ScrollIntoView(this.LogListView.SelectedItem);
                ListViewItem lvi = this.LogListView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
                lvi.BringIntoView();
                lvi.Focus();
                return true;
            }

            return false;
        }

        #endregion
    }
}
