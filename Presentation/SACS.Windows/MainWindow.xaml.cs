using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace SACS.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.VersionLabel.Content = string.Format("v{0}", this.GetType().Assembly.GetName().Version);
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
    }
}