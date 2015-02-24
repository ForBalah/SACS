using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using SACS.DataAccessLayer.Models;
using SACS.Windows.ViewModels;

namespace SACS.Windows.Controls
{
    /// <summary>
    /// Interaction logic for AnalyticsControl.xaml
    /// </summary>
    public partial class AnalyticsControl : UserControl, IAnalyticsView
    {
        private readonly AnalyticsPresenter _presenter;
        private DateTime _FromDate;
        private DateTime _ToDate;
        private List<ScatterSeries> _series = new List<ScatterSeries>();
        protected List<AppPerformanceViewModel> _appPerformances = new List<AppPerformanceViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsControl"/> class.
        /// </summary>
        public AnalyticsControl()
        {
            InitializeComponent();
            this._presenter = new AnalyticsPresenter(this, new WebApiClientFactory());
            this.FromDate = DateTime.Today.AddMonths(-1);
            this.ToDate = DateTime.Today.AddDays(1);
        }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public DateTime FromDate
        {
            get
            {
                return this._FromDate;
            }

            set
            {
                this._FromDate = value;
                this.FromDatePicker.SelectedDate = value;
            }
        }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public DateTime ToDate
        {
            get
            {
                return this._ToDate;
            }

            set
            {
                this._ToDate = value;
                this.ToDatePicker.SelectedDate = value;
            }
        } 

        #region Methods

        /// <summary>
        /// Sets the performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetPerformanceData(IDictionary<string, IList<AppPerformance>> data)
        {
            this._appPerformances.Clear();
            this._appPerformances.AddRange(data.ToList().Select(d => new AppPerformanceViewModel(d.Key, d.Value)));
            this.UpdateChartData();
            this.UpdateTreeView();
        }

        /// <summary>
        /// Updates the TreeView.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void UpdateTreeView()
        {
            this.DataTreeView.Items.Clear();
            foreach (var dataItem in this._appPerformances.OrderBy(a => a.Name))
            {
                var treeItem = new TreeViewItem();
                treeItem.Header = new Label { Content = dataItem.Name, FontWeight = FontWeights.Bold };

                this.DataTreeView.Items.Add(treeItem);

                foreach (var perfItem in dataItem.Data)
                {
                    var childItem = new TreeViewItem
                    {
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                    };

                    var panel = new DockPanel
                    {
                        LastChildFill = true
                    };
                    CreateLabel(panel, perfItem.StartTimeString, 180);
                    CreateLabel(panel, perfItem.EndTimeString, 180);
                    CreateLabel(panel, perfItem.Message, null);
                    childItem.Header = panel;

                    treeItem.Items.Add(childItem);
                }
            }
            
            this.DataTreeView.UpdateLayout();
        }

        /// <summary>
        /// Creates the label.
        /// </summary>
        /// <param name="panel">The panel.</param>
        /// <param name="content">The content.</param>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        private UIElement CreateLabel(Panel panel, string content, double? width)
        {
            var label = new Label
            {
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(1),
                Content = content,
            };

            panel.Children.Add(label);

            if (width.HasValue)
            {
                label.Width = width.Value;
                DockPanel.SetDock(label, Dock.Left);
            }

            return label;
        }

        /// <summary>
        /// Sets the chart data.
        /// </summary>
        /// <param name="data">The data.</param>
        private void UpdateChartData()
        {
            foreach (var dataItem in this._appPerformances.Where(d => d.IsSelected))
            {
                ScatterSeries series1 = this._series.FirstOrDefault(s => (string)s.Title == dataItem.Name);
                if (series1 == null)
                {
                    series1 = new ScatterSeries
                    {
                        Title = dataItem.Name,
                        IsSelectionEnabled = false,
                        IndependentValuePath = "StartTime",
                        DependentValuePath = "Duration",
                        TransitionDuration = TimeSpan.MinValue
                    };
                    this.DurationChart.Series.Add(series1);
                    this._series.Add(series1);
                }

                series1.ItemsSource = dataItem.Data;
            }

            for (int i = this._series.Count - 1; i >= 0; i--)
            {
                ScatterSeries seriesToDelete = this._series[i];
                if (!this._appPerformances.Where(d => d.IsSelected).Select(d => d.Name).Contains(seriesToDelete.Title))
                {
                    this.DurationChart.Series.Remove(seriesToDelete);
                    this._series.Remove(seriesToDelete);
                }
            }
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title"></param>
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
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            if (this.FromDate > this.ToDate)
            {
                MessageBox.Show("From date cannot be greater than To date", "Invalid date", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                this._presenter.LoadData(this.FromDate, this.ToDate);
            }
        }

        #endregion

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Update();
        }

        /// <summary>
        /// Handles the Click event of the RefreshButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.Update();
        }

        /// <summary>
        /// Handles the CalendarClosed event of the FromDatePicker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void FromDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // TODO: Ideally i want to use Bindings but they need to be set up properly first
            this._FromDate = this.FromDatePicker.SelectedDate ?? DateTime.MinValue;
        }

        /// <summary>
        /// Handles the CalendarClosed event of the ToDatePicker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ToDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // TODO: Ideally i want to use Bindings but they need to be set up properly first
            this._ToDate = this.ToDatePicker.SelectedDate ?? DateTime.MinValue;
        }
    }
}
