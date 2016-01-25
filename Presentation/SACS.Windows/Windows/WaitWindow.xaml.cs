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

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitWindow"/> class.
        /// </summary>
        public WaitWindow()
        {
            this.InitializeComponent();
            this.Closed += this.WaitWindow_Closed;
        }

        /// <summary>
        /// Gets the single instance of this window for use throughout the app.
        /// </summary>
        public static WaitWindow SingleInstance
        {
            get
            {
                lock (syncLock)
                {
                    if (_singletonWindow == null || _singletonWindow._isClosed)
                    {
                        _singletonWindow = new WaitWindow();
                    }
                }

                return _singletonWindow;
            }
        }

        /// <summary>
        /// Handles the closed event of the wait window control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected void WaitWindow_Closed(object sender, EventArgs e)
        {
            this._isClosed = true;
        }

        /// <summary>
        /// Shows the form, forcing it to draw while doing so.
        /// </summary>
        public void ShowDrawn()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => this.Show()));
        }
    }
}