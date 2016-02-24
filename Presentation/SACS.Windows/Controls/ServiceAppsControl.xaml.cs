using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using SACS.BusinessLayer.BusinessLogic.Schedule;
using SACS.BusinessLayer.BusinessLogic.Validation;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;
using SACS.Common.Helpers;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;
using SACS.Windows.Windows;
using Enums = SACS.Common.Enums;

using Models = SACS.DataAccessLayer.Models;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ServiceAppsControl.xaml
    /// </summary>
    public partial class ServiceAppsControl : UserControl, IServiceAppView
    {
        //// ApplicationCommands
        //// ComponentCommands
        //// MediaCommands.
        //// NavigationCommands
        //// SystemCommands
        private List<ServiceApp> _serviceApps = new List<ServiceApp>();

        private bool _inEditMode = false;
        private ServiceApp _selectedServiceApp;
        private ServiceAppPresenter _presenter;
        private AnalyticsWindow _analytics;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppsControl"/> class.
        /// </summary>
        public ServiceAppsControl()
        {
            this.InitializeComponent();
            this._presenter = new ServiceAppPresenter(this, new WebApiClientFactory());
            this.ServiceAppListView.ItemsSource = this._serviceApps;
        }

        public event EventHandler<string> GeneralStatusChange;

        #region Properties

        public ICommand Export { get; set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the CancelServiceAppButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectServiceApp(this._selectedServiceApp, true);
        }

        /// <summary>
        /// Handles the CanExecute event of the CommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !this._inEditMode;
        }

        /// <summary>
        /// Handles the Click event of the AppFilePathSelectButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void AppFilePathSelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable Files (*.exe)|*.exe|All files (*.*)|*.*"; // "Assemblies (*.exe;*.dll)|*.exe;*.dll|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                this.AppFilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Handles the Click event of the EditServiceAppButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void EditServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectServiceApp(this._selectedServiceApp, false);
        }

        /// <summary>
        /// Handles the Executed event of the NewCommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ServiceAppListView.SelectedItem = null;
            this.SelectServiceApp(null, false);
        }

        /// <summary>
        /// Handles the Executed event of the RefreshCommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void RefreshCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WaitWindow.SingleInstance.ShowDrawn(true);
            this._presenter.LoadServiceApps();
            WaitWindow.SingleInstance.TryClose();
        }

        /// <summary>
        /// Handles the Executed event of the ExportCommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void ExportCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text (Tab Delimited)|*.txt|Text (Comma Delimited)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string exportContents = this._presenter.ExportServiceAppList(Path.GetExtension(saveFileDialog.FileName));
                    File.WriteAllText(saveFileDialog.FileName, exportContents);
                    MessageBox.Show("List exported successfuly.", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    this.ShowException("Failed to export", ex);
                }
            }
        }

        /// <summary>
        /// Handles the Executed event of the ViewAnalyticsCommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void ViewAnalyticsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this._analytics == null || !this._analytics.IsVisible)
            {
                this._analytics = new AnalyticsWindow();
            }

            this._analytics.ShowActivated = true;
            this._analytics.Show();
            if (this._analytics.WindowState == WindowState.Minimized)
            {
                this._analytics.WindowState = WindowState.Normal;
            }

            this._analytics.Focus();
        }

        /// <summary>
        /// Handles the Executed event of the HelpCommandBinding control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void HelpCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.OpenHelp();
        }

        /// <summary>
        /// Handles the Click event of the IdentitySelectButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void IdentitySelectButton_Click(object sender, RoutedEventArgs e)
        {
            AccountSelectWindow accountWindow = new AccountSelectWindow();
            accountWindow.IsCustomAccount = !string.IsNullOrWhiteSpace(this.IdentityLabel.Text);
            accountWindow.Username = this.IdentityLabel.Text;
            if (accountWindow.ShowDialog() == true)
            {
                if (accountWindow.IsCustomAccount)
                {
                    this.IdentityLabel.Text = accountWindow.Username;
                    this.PasswordHiddenLabel.Text = accountWindow.EncryptedPassword;
                }
                else
                {
                    this.IdentityLabel.Text = null;
                    this.PasswordHiddenLabel.Text = null;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ScheduleSelectButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ScheduleSelectButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.UpdateWith(!string.IsNullOrWhiteSpace(this.ScheduleHiddenLabel.Text) ? this.ScheduleHiddenLabel.Text : ScheduleUtility.DefaultCrontab);
            if (scheduleWindow.ShowDialog() == true)
            {
                this.ScheduleHiddenLabel.Text = scheduleWindow.Schedule;
                this.ScheduleLabel.Text = ScheduleUtility.GetFullDescription(scheduleWindow.Schedule);
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ServiceAppListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ServiceAppListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ServiceApp item = (sender as ListView).SelectedItem as ServiceApp;
            this.SelectServiceApp(item, true);
        }

        /// <summary>
        /// Handles the click event of the StartServiceAppButton
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StartServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedServiceApp != null)
            {
                this._presenter.StartServiceApp(this._selectedServiceApp);
            }
        }

        /// <summary>
        /// Handles the Click event of the StopServiceAppButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StopServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedServiceApp != null)
            {
                this._presenter.StopServiceApp(this._selectedServiceApp);
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveServiceAppButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceApp serviceApp = this.BuildServiceAppFromInput();
            if (serviceApp != null)
            {
                this._presenter.UpdateServiceApp(serviceApp);
                this._inEditMode = false;
                this.ToggleEditFieldVisibility(false);
            }
        }

        /// <summary>
        /// Handles the Click event of the DeleteServiceAppButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DeleteServiceAppButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this Service App?", "Remove Service App", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this._presenter.RemoveServiceApp(this.ServiceAppNameTextBox.Text ?? (string)this.ServiceAppNameLabel.Content);
                this._inEditMode = false;
                this.ToggleEditFieldVisibility(false);
            }
        }

        /// <summary>
        /// Handles the Click event of the RunButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedServiceApp != null)
            {
                this._presenter.RunServiceApp(this._selectedServiceApp);
                MessageBox.Show(string.Format("{0} is queued to run.", this._selectedServiceApp.Name), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            WaitWindow.SuppressDialog = false;
            WaitWindow.SingleInstance.ShowDrawn(true);
            this.LoadComboBoxes();
            this._presenter.LoadServiceApps(this.IsVisible);
            WaitWindow.SingleInstance.TryClose();
            this.ServiceAppListView.Focus();
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Shows the exception.
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
        /// Binds the service apps.
        /// </summary>
        /// <param name="list">The list.</param>
        public void BindServiceApps(IList<Models.ServiceApp> list)
        {
            if (list != null)
            {
                this.MergeServiceAppList(list);
            }
        }

        /// <summary>
        /// Sets the status bar message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetStatusMessage(string message)
        {
            if (this.GeneralStatusChange != null)
            {
                this.GeneralStatusChange(this, message);
            }
        }

        /// <summary>
        /// Selects the specified service app, or deselects if null is passed in
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        /// <param name="isReadOnly">If the service should be shown as read-only. Default is true.</param>
        public void SelectServiceApp(ServiceApp serviceApp, bool isReadOnly = true)
        {
            this._inEditMode = !isReadOnly;
            this.ToggleEditFieldVisibility(!isReadOnly);
            this._selectedServiceApp = serviceApp;
            this.ShowServiceAppDetails(this._selectedServiceApp, isReadOnly);
        }

        /// <summary>
        /// Builds the service application from input.
        /// </summary>
        /// <returns></returns>
        private ServiceApp BuildServiceAppFromInput()
        {
            if (this.ValidateInput())
            {
                ServiceApp newServiceApp = new ServiceApp
                {
                    Name = this.ServiceAppNameTextBox.Text,
                    StartupTypeEnum = (Enums.StartupType)this.StartupTypeComboBox.SelectedValue,
                    Environment = this.ServiceAppEnvironmentTextBox.Text,
                    Description = this.DescriptionTextBox.Text,
                    AppFilePath = this.AppFilePathTextBox.Text,
                    SendSuccessNotification = this.SendSuccessCheckBox.IsChecked ?? false,
                    Username = this.IdentityLabel.Text,
                    Password = this.PasswordHiddenLabel.Text,
                    Schedule = this.ScheduleHiddenLabel.Text
                };

                return newServiceApp;
            }

            return null;
        }

        /// <summary>
        /// Loads the combo boxes.
        /// </summary>
        private void LoadComboBoxes()
        {
            this.StartupTypeComboBox.ItemsSource = EnumHelper.GetEnumList<Enums.StartupType>();
        }

        /// <summary>
        /// Merges the service app list with the existing field.
        /// </summary>
        /// <param name="list">The list.</param>
        private void MergeServiceAppList(IList<Models.ServiceApp> list)
        {
            this._serviceApps.Clear();
            this._serviceApps.AddRange(list);
            this._serviceApps.Sort(ServiceApp.Comparer);

            string app = this._selectedServiceApp != null ? this._selectedServiceApp.Name : null;
            ICollectionView view = CollectionViewSource.GetDefaultView(this.ServiceAppListView.ItemsSource);
            view.Refresh();

            this.ServiceAppListView.SelectedIndex = -1; // deselect to force correct refresh
            if (app != null)
            {
                for (int i = 0; i < this.ServiceAppListView.Items.Count; i++)
                {
                    var serviceApp = this.ServiceAppListView.Items[i] as ServiceApp;
                    if (serviceApp.Name == app)
                    {
                        this.ServiceAppListView.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Shows the service app details.
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        /// <param name="isReadOnly">If set to <c>true</c> show the details as read only.</param>
        private void ShowServiceAppDetails(ServiceApp serviceApp, bool isReadOnly)
        {
            if (serviceApp == null)
            {
                serviceApp = new Models.ServiceApp
                {
                    StartupTypeEnum = Enums.StartupType.NotSet,
                    Schedule = string.Empty
                };
            }

            this.ServiceAppNameLabel.Content = this.ServiceAppNameTextBox.Text = serviceApp.Name;
            this.MessageLabel.Text = serviceApp.LastMessage;

            this.StartServiceAppButton.IsEnabled = serviceApp.CanStart && isReadOnly;
            this.StopServiceAppButton.IsEnabled = serviceApp.CanStop && isReadOnly;
            this.RunButton.IsEnabled = serviceApp.CanRun && isReadOnly;

            this.StartupTypeLabel.Text = serviceApp.StartupTypeEnum.GetName();
            this.StartupTypeComboBox.SelectedValue = serviceApp.StartupTypeEnum;
            this.ServiceAppDescriptionLabel.Text = this.DescriptionTextBox.Text = serviceApp.Description;
            this.ServiceAppEnvironmentLabel.Text = this.ServiceAppEnvironmentTextBox.Text = serviceApp.Environment;
            this.AppFilePathLabel.Text = this.AppFilePathTextBox.Text = serviceApp.AppFilePath;
            this.SendSuccessCheckBox.IsChecked = serviceApp.SendSuccessNotification;
            this.IdentityLabel.Text = this.IdentityLabel.Text = serviceApp.Username;
            this.PasswordHiddenLabel.Text = serviceApp.Password;
            this.ScheduleLabel.Text = ScheduleUtility.GetFullDescription(serviceApp.Schedule);
            this.ScheduleHiddenLabel.Text = serviceApp.Schedule;

            this.EditServiceAppButton.IsEnabled = !serviceApp.CanRun;
            this.EditMessageTextBlock.Text = serviceApp.CanRun ? "Stop the app before making changes" : null;
        }

        /// <summary>
        /// Toggles the edit field visibility.
        /// </summary>
        /// <param name="isEdit">If set to <c>true</c> [is edit].</param>
        private void ToggleEditFieldVisibility(bool isEdit)
        {
            this.ServiceAppListView.IsEnabled = !isEdit;
            this.StartServiceAppButton.IsEnabled = false;
            this.StopServiceAppButton.IsEnabled = false;
            this.RunButton.IsEnabled = false;
            this.ServiceAppNameTitleLabel.Visibility = MapVisibility(isEdit);
            this.ServiceAppNameLabel.Visibility = MapVisibility(!isEdit || this._selectedServiceApp != null);
            this.ServiceAppNameTextBox.Visibility = MapVisibility(isEdit && this._selectedServiceApp == null);
            this.StartupTypeLabel.Visibility = MapVisibility(!isEdit);
            this.StartupTypeComboBox.Visibility = MapVisibility(isEdit);
            this.ServiceAppEnvironmentLabel.Visibility = MapVisibility(!isEdit);
            this.ServiceAppEnvironmentTextBox.Visibility = MapVisibility(isEdit);
            this.ServiceAppDescriptionLabel.Visibility = MapVisibility(!isEdit);
            this.DescriptionTextBox.Visibility = MapVisibility(isEdit);
            this.AppFilePathLabel.Visibility = MapVisibility(!isEdit);
            this.AppFilePathDockPanel.Visibility = MapVisibility(isEdit);
            this.SendSuccessCheckBox.IsEnabled = isEdit;
            this.IdentitySelectButton.Visibility = MapVisibility(isEdit);
            this.ScheduleSelectButton.Visibility = MapVisibility(isEdit);
            this.EditServiceAppButton.Visibility = MapVisibility(!isEdit);
            this.SaveServiceAppButton.Visibility = MapVisibility(isEdit);
            this.RemoveServiceAppButton.Visibility = MapVisibility(isEdit && this._selectedServiceApp != null);
            this.CancelServiceAppButton.Visibility = MapVisibility(isEdit);

            // Also need to place the first focus
            if (isEdit)
            {
                this.ServiceAppNameTextBox.Focus();
            }
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            ServiceAppValidator validator = new ServiceAppValidator();
            validator.ValidateAppName(this.ServiceAppNameTextBox.Text);
            validator.ValidateStartupType((Enums.StartupType)this.StartupTypeComboBox.SelectedValue);
            validator.ValidateEnvironmentName(this.ServiceAppEnvironmentTextBox.Text);
            validator.ValidateDescription(this.DescriptionTextBox.Text);
            validator.ValidateAppFilePath(this.AppFilePathTextBox.Text);
            validator.ValidateSchedule(this.ScheduleHiddenLabel.Text);

            if (validator.ErrorMessages.Any())
            {
                MessageBox.Show("• " + string.Join(Environment.NewLine + "• ", validator.ErrorMessages), "The following validation errors occured", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Quick method to map the visibility which lowers the cyclomatic complexity.
        /// </summary>
        /// <param name="isVisible">If set to <c>true</c> [is visible].</param>
        /// <returns></returns>
        private static Visibility MapVisibility(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion Methods
    }
}