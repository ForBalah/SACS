using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using SACS.Common.Enums;
using SACS.Common.Helpers;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ServerStatusControl.xaml
    /// </summary>
    public partial class ServerStatusControl : UserControl, IServerStatusView
    {
        private readonly ServerStatusPresenter _presenter;
        private readonly Timer _timer;
        private DateTime? _startTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerStatusControl"/> class.
        /// </summary>
        public ServerStatusControl()
        {
            this.InitializeComponent();
            this._presenter = new ServerStatusPresenter(this);
            this._timer = new Timer(1000);
            this._timer.Elapsed += this.Timer_Elapsed;
            this._timer.Start();
        }

        #region Event Handlers

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action<string> updateLabel = new Action<string>(a =>
            {
                this.UpTimeLabel.Content = a;
            });

            if (this._startTime.HasValue)
            {
                this.UpTimeLabel.Dispatcher.BeginInvoke(updateLabel, (DateTime.Now - this._startTime.Value).ToString(@"d\ \d\a\y\s\ hh\:mm\:ss"));
            }
            else
            {
                this.UpTimeLabel.Dispatcher.BeginInvoke(updateLabel, "--:--:--");
            }
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._presenter.LoadStatus();
        }

        /// <summary>
        /// Handles the Click event of the RefreshButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.StartServerButton.IsEnabled = false;
            this.StopServerButton.IsEnabled = false;
            this._presenter.LoadStatus();
        }

        /// <summary>
        /// Handles the Click event of the StartServerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            this._presenter.StartServer();
        }

        /// <summary>
        /// Handles the Click event of the StopServerButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StopServerButton_Click(object sender, RoutedEventArgs e)
        {
            this._presenter.StopServer();
        }

        /// <summary>
        /// Handles the Click event of the ViewExceptionButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewExceptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.ExceptionPopup.IsOpen = !this.ExceptionPopup.IsOpen;
        }

        /// <summary>
        /// Handles the Click event of the PopupCloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PopupCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.ExceptionPopup.IsOpen = false;
        } 

        #endregion

        #region Methods

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="serverStatus">The server status.</param>
        /// <param name="message">The message.</param>
        /// <param name="startTime">The server start time</param>
        /// <param name="e">The e.</param>
        public void SetStatus(ServerStatus serverStatus, string message, DateTime? startTime, Exception e)
        {
            this.StatusDescription.Content = message;
            this._startTime = startTime;

            switch (serverStatus)
            {
                case ServerStatus.Started:
                    this.ServerStatusEllipse.Fill = Brushes.DarkGreen;
                    this.StartServerButton.IsEnabled = false;
                    this.StopServerButton.IsEnabled = true;
                    break;
                case ServerStatus.Starting:
                    this.ServerStatusEllipse.Fill = Brushes.Yellow;
                    this.StartServerButton.IsEnabled = false;
                    this.StopServerButton.IsEnabled = false;
                    break;
                case ServerStatus.Stopped:
                    this.ServerStatusEllipse.Fill = Brushes.DarkOrange;
                    this.StartServerButton.IsEnabled = true;
                    this.StopServerButton.IsEnabled = false;
                    break;
                case ServerStatus.Error:
                    this.ServerStatusEllipse.Fill = Brushes.Red;
                    this.StartServerButton.IsEnabled = false;
                    this.StopServerButton.IsEnabled = false;
                    break;
                case ServerStatus.Unknown:
                default:
                    this.ServerStatusEllipse.Fill = Brushes.Black;
                    this.StartServerButton.IsEnabled = false;
                    this.StopServerButton.IsEnabled = false;
                    break;
            }

            if (e != null)
            {
                this.ViewExceptionButton.Visibility = System.Windows.Visibility.Visible;
                this.ExceptionPopupTextBox.Text = ExceptionHelper.GetExceptionDetails(e);
            }
            else
            {
                this.ViewExceptionButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Sets the type of the startup.
        /// </summary>
        /// <param name="startupType">Type of the startup.</param>
        public void SetStartupType(string startupType)
        {
            if (!string.IsNullOrWhiteSpace(startupType))
            {
                this.StartupTypeLabel.Content = "Server start up type: " + startupType;
            }
            else
            {
                this.StartupTypeLabel.Content = string.Empty;
            }

            if (!startupType.Contains("AUTOMATIC"))
            {
                this.StartupTypeLabel.Foreground = Brushes.Red;
            }
            else
            {
                this.StartupTypeLabel.Foreground = Brushes.Black;
            }
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        /// <exception cref="System.NotImplementedException">Not yet implemented.</exception>
        public void ShowException(string title, Exception e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
