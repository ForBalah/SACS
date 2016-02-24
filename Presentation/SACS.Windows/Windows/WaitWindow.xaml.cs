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
using System.Windows.Threading;

namespace SACS.Windows.Windows
{
    /// <summary>
    /// Interaction logic for WaitWindow.xaml
    /// </summary>
    public partial class WaitWindow : Window
    {
        private static WaitWindow _singletonWindow;
        private static object syncLock = new object();
        private bool _isClosed = false;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitWindow"/> class.
        /// </summary>
        public WaitWindow()
        {
            this.InitializeComponent();
            this.Closed += this.WaitWindow_Closed;
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets the single instance of this window for use throughout the app.
        /// </summary>
        public static WaitWindow SingleInstance
        {
            get
            {
                lock (syncLock)
                {
                    if (_singletonWindow == null || (_singletonWindow._isClosed && Application.Current != null))
                    {
                        _singletonWindow = new WaitWindow();
                    }
                }

                return _singletonWindow;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to suppress the showing of the dialog
        /// </summary>
        public static bool SuppressDialog { get; set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the closed event of the wait window control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected void WaitWindow_Closed(object sender, EventArgs e)
        {
            this._isClosed = true;
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Shows the window, forcing it to draw while doing so.
        /// </summary>
        /// <param name="overrideDialogSuppression">If set to <c>true</c>, will show the dialog regardless of if
        /// it has been requested to suppress it.</param>
        public void ShowDrawn(bool overrideDialogSuppression)
        {
            if (!SuppressDialog || overrideDialogSuppression)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)this.TryShowWindow);
            }
        }

        /// <summary>
        /// Attempts to show the window... if the application will allow it
        /// </summary>
        private void TryShowWindow()
        {
            try
            {
                if (!this._isClosed)
                {
                    this.Show();
                }
            }
            catch
            {
                // TODO: log that the window cannot be shown
            }
        }

        /// <summary>
        /// Attempts to close the window
        /// </summary>
        public void TryClose()
        {
            try
            {
                if (!this._isClosed)
                {
                    this.Close();
                }
            }
            catch
            {
                // TODO: log that the close failed.
            }
        }

        #endregion Methods
    }
}