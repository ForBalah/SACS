using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using SACS.Windows.Windows;

namespace SACS.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WaitWindow _waitWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.VersionLabel.Content = string.Format("v{0}", this.GetType().Assembly.GetName().Version);
            this._waitWindow = new WaitWindow();
        }

        /// <summary>
        /// Opens the help file. Designed to be callable from any control
        /// </summary>
        public static void OpenHelp()
        {
            string assemblyLoc = Assembly.GetAssembly(typeof(MainWindow)).Location;
            string helpLoc = string.Format(@"{0}\Help\SACS Help.mht", System.IO.Path.GetDirectoryName(assemblyLoc));
            Process.Start("file:///" + helpLoc);
        }

        /// <summary>
        /// Services the apps control_ general status change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void ServiceAppsControl_GeneralStatusChange(object sender, string e)
        {
            this.StatusTextBlock.Text = e;
        }

        /// <summary>
        /// Handles the Click event of the MainHelpButton control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void MainHelpButton_Click(object sender, RoutedEventArgs e)
        {
            OpenHelp();
        }

        /// <summary>
        /// Handles the Closed event of the main Window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}