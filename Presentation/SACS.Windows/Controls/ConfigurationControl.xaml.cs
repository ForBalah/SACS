using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
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
using SACS.Common.Configuration;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ConfigurationControl.xaml
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationControl"/> class.
        /// </summary>
        public ConfigurationControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the StackPanel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowConfigurationDetails();
        }

        /// <summary>
        /// Shows the configuration details.
        /// </summary>
        private void ShowConfigurationDetails()
        {
            IApplicationSettings settings = ApplicationSettings.Current;
            this.UrlLabel.Text = "SACS Service URL: " + settings.WebApiBaseAddress;
            this.NameLabel.Text = "Windows Service Name: " + settings.ServiceName;

            try
            {
                // get service status
                ServiceController sc = new ServiceController(ApplicationSettings.Current.ServiceName);
                this.DisplayNameLabel.Text = "Display Name: " + sc.DisplayName;
            }
            catch
            {
            }
        }
    }
}
