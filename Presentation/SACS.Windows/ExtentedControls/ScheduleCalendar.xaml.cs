using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using SACS.BusinessLayer.BusinessLogic.Schedule;
using SACS.BusinessLayer.Extensions;

namespace SACS.Windows.ExtentedControls
{
    /// <summary>
    /// Interaction logic for ScheduleCalendar.xaml
    /// </summary>
    public partial class ScheduleCalendar : UserControl
    {
        private string _schedule;
        private DateTime? _currentDay;
        private List<DateTime> _nextOccurences;

        public ScheduleCalendar()
        {
            this.InitializeComponent();
            this.DayCalendar.DisplayDateStart = DateTime.Today;
            this.DayCalendar.SelectedDate = DateTime.Today;
            this.DayCalendar.SelectionMode = CalendarSelectionMode.MultipleRange;
            this._nextOccurences = new List<DateTime>();
        }

        /// <summary>
        /// Occurs when selected date has been changed in the calendar.
        /// </summary>
        public event EventHandler SelectedDateChanged;

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        public string Schedule
        {
            get
            {
                return this._schedule;
            }

            set
            {
                this._schedule = value;

                if (this.IsVisible)
                {
                    this.HiglightDays();
                }
            }
        }

        #region Event Handlers
        
        /// <summary>
        /// Handles the SelectedDatesChanged event of the DayCalendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void DayCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.HighlightHours((DateTime)e.AddedItems[0]);
            }

            if (this.SelectedDateChanged != null)
            {
                this.SelectedDateChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the DisplayDateChanged event of the DayCalendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CalendarDateChangedEventArgs"/> instance containing the event data.</param>
        private void DayCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            this.HiglightDays();
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.HiglightDays();
            }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Higlights the days.
        /// </summary>
        private void HiglightDays()
        {
            this.NormalizeDisplayDate();
            this._nextOccurences = ScheduleUtility.GetNextOccurrences(this.Schedule, this.DayCalendar.DisplayDate, this.DayCalendar.DisplayDate.LastDayOfMonth().AddDays(1)).ToList();

            DateTime? selectedDate = this.DayCalendar.SelectedDate ?? DateTime.Today;
            this.DayCalendar.SelectedDates.Clear();
            this.DayCalendar.BlackoutDates.Clear();
            int firstDay = this.DayCalendar.DisplayDate.Day;
            var allDays = Enumerable.Range(firstDay, this.DayCalendar.DisplayDate.LastDayOfMonth().Day - firstDay + 1);
            var occurenceDays = this._nextOccurences.Select(o => o.Day).Distinct();
            var blockedDays = allDays.Except(occurenceDays);

            foreach (int day in blockedDays)
            {
                CalendarDateRange blockedRange = new CalendarDateRange(this.DayCalendar.DisplayDate.AddDays(day - firstDay));
                this.DayCalendar.BlackoutDates.Add(blockedRange);
            }

            // reselect date if we can (will kick of date selection)
            if (selectedDate.HasValue && !this.DayCalendar.BlackoutDates.Contains(selectedDate.Value))
            {
                this.DayCalendar.SelectedDate = selectedDate;
            }
        }

        /// <summary>
        /// Highlights the hours.
        /// </summary>
        /// <param name="currentDay">The current day.</param>
        private void HighlightHours(DateTime currentDay)
        {
            if (this._nextOccurences != null)
            {
                this._currentDay = currentDay;
                var hours = this._nextOccurences.Where(o => o >= this._currentDay.Value && o < this._currentDay.Value.AddDays(1));
                SolidColorBrush hourBrush = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));

                this.HourCanvas.Children.Clear();
                this.MinuteCanvas.Children.Clear();
                foreach (var hour in hours.Select(hd => hd.Hour).Distinct())
                {
                    var rectangle = new Rectangle();
                    rectangle.Fill = hourBrush;
                    rectangle.Width = this.HourCanvas.ActualWidth;
                    rectangle.Height = this.HourCanvas.ActualHeight / 24d;

                    Canvas.SetLeft(rectangle, 0);
                    Canvas.SetTop(rectangle, this.HourCanvas.ActualHeight * hour / 24d);
                    this.HourCanvas.Children.Add(rectangle);
                }

                // for convenience take the last hour and map the minutes
                int? lastHour = hours.Select(hd => hd.Hour).LastOrDefault();
                if (lastHour.HasValue)
                {
                    this.HighlightMinutes(this._currentDay.Value.AddHours(lastHour.Value));
                }
            }
        }

        /// <summary>
        /// Highlights the minutes.
        /// </summary>
        /// <param name="currentHour">The current hour.</param>
        private void HighlightMinutes(DateTime currentHour)
        {
            if (this._nextOccurences != null)
            {
                var minutes = this._nextOccurences.Where(o => o >= currentHour && o < currentHour.AddHours(1));
                SolidColorBrush minuteBrush = new SolidColorBrush(Color.FromArgb(255, 0, 122, 204));

                this.MinuteCanvas.Children.Clear();
                foreach (var minute in minutes.Select(m => m.Minute))
                {
                    var rectangle = new Rectangle();
                    rectangle.Fill = minuteBrush;
                    rectangle.Width = this.MinuteCanvas.ActualWidth;
                    rectangle.Height = this.MinuteCanvas.ActualHeight / 60d;

                    Canvas.SetLeft(rectangle, 0);
                    Canvas.SetTop(rectangle, this.MinuteCanvas.ActualHeight * minute / 60d);
                    this.MinuteCanvas.Children.Add(rectangle);
                }
            }
        }

        /// <summary>
        /// Normalizes the display date.
        /// </summary>
        private void NormalizeDisplayDate()
        {
            int beginPeriod = ((this.DayCalendar.DisplayDateStart ?? DateTime.Today).Year * 100) + (this.DayCalendar.DisplayDateStart ?? DateTime.Today).Month;
            int displayPeriod = (this.DayCalendar.DisplayDate.Year * 100) + this.DayCalendar.DisplayDate.Month;

            if (displayPeriod > beginPeriod)
            {
                this.DayCalendar.DisplayDate = new DateTime(this.DayCalendar.DisplayDate.Year, this.DayCalendar.DisplayDate.Month, 1);
            }
            else
            {
                this.DayCalendar.DisplayDate = this.DayCalendar.DisplayDateStart ?? DateTime.Today;
            }
        }

        #endregion
    }
}
