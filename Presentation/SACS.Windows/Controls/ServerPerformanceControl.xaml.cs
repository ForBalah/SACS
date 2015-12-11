using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
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

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for ServerPerformanceControl.xaml
    /// </summary>
    public partial class ServerPerformanceControl : UserControl, IServerPerformanceView
    {
        private readonly ServerPerformancePresenter _presenter;
        private AreaSeries _seriesCpu = new AreaSeries();
        private AreaSeries _seriesMenory = new AreaSeries();
        private Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPerformanceControl"/> class.
        /// </summary>
        public ServerPerformanceControl()
        {
            this.InitializeComponent();
            this._presenter = new ServerPerformancePresenter(this, new WebApiClientFactory());
            this.SetupSeries();
            this._timer = new Timer(1.5 * 60000);
            this._timer.Elapsed += this.Timer_Elapsed;
            this._timer.Start();
        }

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the GraphRefreshLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void GraphRefreshLink_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke((Action)this.LoadGraphs);
            }
        }

        /// <summary>
        /// Handles the Elapsed event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsVisible)
            {
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke((Action)this.LoadGraphs);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadGraphs();
        }

        #endregion Event Handlers

        /// <summary>
        /// Sets the cpu performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetCpuPerformanceData(IList<DataAccessLayer.Models.SystemPerformance> data)
        {
            this._seriesCpu.ItemsSource = data;
        }

        /// <summary>
        /// Sets the memory performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetMemoryPerformanceData(IList<DataAccessLayer.Models.SystemPerformance> data)
        {
            this._seriesMenory.ItemsSource = data;
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            ////MessageBox.Show("chart error: " + e.Message);
        }

        /// <summary>
        /// Loads the graphs.
        /// </summary>
        private void LoadGraphs()
        {
            double lookBackDays;
            if (double.TryParse(ConfigurationManager.AppSettings["Performance.LookBackDays"], out lookBackDays))
            {
                DateTime toDate = DateTime.Now.AddMinutes(1);
                this._presenter.LoadData(toDate.AddDays(-lookBackDays), toDate);
            }
        }

        /// <summary>
        /// Setups the cpu and memory chart series.
        /// </summary>
        private void SetupSeries()
        {
            // CPU
            this.CpuChart.Series.Clear();
            this._seriesCpu.TransitionDuration = TimeSpan.MinValue;
            this._seriesCpu.Title = "CPU";
            this._seriesCpu.IsSelectionEnabled = false;
            this._seriesCpu.IndependentValuePath = "AuditTime";
            this._seriesCpu.DependentValuePath = "Value";
            this.CpuChart.Series.Add(this._seriesCpu);

            // Memory
            this.MemoryChart.Series.Clear();
            this._seriesMenory.TransitionDuration = TimeSpan.MinValue;
            this._seriesMenory.Title = "Memory";
            this._seriesMenory.IsSelectionEnabled = false;
            this._seriesMenory.IndependentValuePath = "AuditTime";
            this._seriesMenory.DependentValuePath = "Value";
            this.MemoryChart.Series.Add(this._seriesMenory);
        }
    }
}