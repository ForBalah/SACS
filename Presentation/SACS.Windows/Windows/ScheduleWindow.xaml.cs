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
using SACS.BusinessLayer.BusinessLogic.Schedule;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;

namespace SACS.Windows.Windows
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window, IScheduleSelectorView
    {
        private readonly ScheduleSelectorPresenter _presenter;
        private bool _isUpdating = false; // TODO: not ideal but this is used to prevent multiple updates because so many change events are bound to

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleWindow"/> class.
        /// </summary>
        public ScheduleWindow()
        {
            this.InitializeComponent();
            this.ValidationMessages = new List<string>();
            this._presenter = new ScheduleSelectorPresenter(this);
            this._presenter.LoadControl();
        }

        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        public string Schedule
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the validation messages.
        /// </summary>
        /// <value>
        /// The validation messages.
        /// </value>
        public IList<string> ValidationMessages { get; private set; }

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the DayCheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void DayCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshSchedule();
        }

        /// <summary>
        /// Handles the DropDownClosed event of the MinuteMultiSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MinuteMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            this.NormalizeMultiSelection(this.MinuteMultiSelect, this.EveryMinuteRadioButton, this.AtMinuteRadioButton);
            this.RefreshSchedule();
        }

        /// <summary>
        /// Handles the DropDownClosed event of the HourMultiSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HourMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            this.NormalizeMultiSelection(this.HourMultiSelect, this.EveryHourRadioButton, this.AtHourRadioButton);
            this.RefreshSchedule();
        }

        /// <summary>
        /// Handles the DropDownClosed event of the DayMultiSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void DayMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            this.NormalizeMultiSelection(this.DayMultiSelect, this.EveryDayRadioButton, this.AtDayRadioButton);
            this.RefreshSchedule();
        }

        /// <summary>
        /// Handles the DropDownClosed event of the MonthMultiSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MonthMultiSelect_DropDownClosed(object sender, EventArgs e)
        {
            this.NormalizeMultiSelection(this.MonthMultiSelect, this.EveryMonthRadioButton, this.AtMonthRadioButton);
            this.RefreshSchedule();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.ValidationMessagesTextBlock.Text))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cannot save due to validation errors.");
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Handles the Checked event of the RadioButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!this._isUpdating)
            {
                this.RefreshSchedule();
            }
        }

        /// <summary>
        /// Handles the SelectedDateChanged event of the ScheduleCalendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ScheduleCalendar_SelectedDateChanged(object sender, EventArgs e)
        {
            this.SaveButton.Focus();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the controls.
        /// </summary>
        public void PopulateControls()
        {
            this.MinuteMultiSelect.ItemsSource = Enumerable.Range(0, 60).ToDictionary(k => k.ToString(), v => (object)v);
            this.HourMultiSelect.ItemsSource = Enumerable.Range(0, 24).ToDictionary(k => k.ToString(), v => (object)v);
            this.DayMultiSelect.ItemsSource = Enumerable.Range(1, 31).ToDictionary(k => k.ToString(), v => (object)v);
            this.MonthMultiSelect.ItemsSource = new Dictionary<string, object>
            {
                { "January", 1 },
                { "February", 2 },
                { "March", 3 },
                { "April", 4 },
                { "May", 5 },
                { "June", 6 },
                { "July", 7 },
                { "August", 8 },
                { "September", 9 },
                { "October", 10 },
                { "November", 11 },
                { "December", 12 },
            };
        }

        /// <summary>
        /// Updates the schedule selector with the provided schedule
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        public void UpdateWith(string schedule)
        {
            this.Schedule = schedule;
            this.UpdateControls();
            this.RefreshSchedule();
        }

        /// <summary>
        /// Updates the schedule.
        /// </summary>
        public void RefreshSchedule()
        {
            ScheduleBuildResult result = ScheduleUtility.BuildSchedule(
                minutePart: GetSchedulePart(this.MinuteMultiSelect, this.EveryMinuteRadioButton.IsChecked ?? false),
                hourPart: GetSchedulePart(this.HourMultiSelect, this.EveryHourRadioButton.IsChecked ?? false),
                dayPart: GetSchedulePart(this.DayMultiSelect, this.EveryDayRadioButton.IsChecked ?? false),
                monthPart: GetSchedulePart(this.MonthMultiSelect, this.EveryMonthRadioButton.IsChecked ?? false),
                dayOfWeekPart: this.GetDayOfWeekSelection());

            this.Schedule = result.Result;
            this.ValidationMessagesTextBlock.Text = string.Join(Environment.NewLine, result.Messages);
            this.SaveButton.IsEnabled = string.IsNullOrEmpty(this.ValidationMessagesTextBlock.Text);

            this.UpdateDescription();
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        public void ShowException(string title, Exception e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the schedule part.
        /// </summary>
        /// <param name="multiSelectComboBox">The multi select ComboBox.</param>
        /// <param name="isEveryChecked">If set to <c>true</c> return tuple indicating everything is checked.</param>
        /// <returns></returns>
        private static Tuple<IList<int>, bool> GetSchedulePart(MultiSelectComboBox.MultiSelectComboBox multiSelectComboBox, bool isEveryChecked)
        {
            if (multiSelectComboBox.AllItemsChecked)
            {
                return new Tuple<IList<int>, bool>(new List<int>(), true);
            }
            else if (multiSelectComboBox.SelectedItems != null)
            {
                return new Tuple<IList<int>, bool>(multiSelectComboBox.SelectedItems.Select(i => (int)i.Value).ToList(), isEveryChecked);
            }
            else
            {
                return new Tuple<IList<int>, bool>(new List<int>(), isEveryChecked); 
            }
        }

        /// <summary>
        /// Selects the multi select values.
        /// </summary>
        /// <param name="multiSelectComboBox">The multi select ComboBox.</param>
        /// <param name="description">The description.</param>
        private static void SelectMultiSelectValues(MultiSelectComboBox.MultiSelectComboBox multiSelectComboBox, DescriptionDirective description)
        {
            multiSelectComboBox.SetSelectedValues(description.ExpandedValues.Cast<object>().ToList());
        }

        /// <summary>
        /// Gets the dat of week selection.
        /// </summary>
        /// <returns></returns>
        private IList<int> GetDayOfWeekSelection()
        {
            List<int> selection = new List<int>();
            if (this.SundayCheckBox.IsChecked ?? false)
            {
                selection.Add(0);
            }

            if (this.MondayCheckBox.IsChecked ?? false)
            {
                selection.Add(1);
            }

            if (this.TuesdayCheckBox.IsChecked ?? false)
            {
                selection.Add(2);
            }

            if (this.WednesdayCheckBox.IsChecked ?? false)
            {
                selection.Add(3);
            }

            if (this.ThursdayCheckBox.IsChecked ?? false)
            {
                selection.Add(4);
            }

            if (this.FridayCheckBox.IsChecked ?? false)
            {
                selection.Add(5);
            }

            if (this.SaturdayCheckBox.IsChecked ?? false)
            {
                selection.Add(6);
            }

            return selection;
        }

        /// <summary>
        /// Normalizes the multi selection.
        /// </summary>
        /// <param name="multiSelectComboBox">The multi select ComboBox.</param>
        /// <param name="everyRadioButton">The every RadioButton.</param>
        /// <param name="atRadioButton">At RadioButton.</param>
        private void NormalizeMultiSelection(MultiSelectComboBox.MultiSelectComboBox multiSelectComboBox, RadioButton everyRadioButton, RadioButton atRadioButton)
        {
            if (multiSelectComboBox.AllItemsChecked)
            {
                everyRadioButton.IsChecked = true;
            }
            else
            {
                if (!multiSelectComboBox.SelectedItems.Any())
                {
                    multiSelectComboBox.SetSelectedValue(multiSelectComboBox.ItemsSource.First().Value);
                }
                else if (multiSelectComboBox.SelectedItems.Count > 1)
                {
                    atRadioButton.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// Updates the description.
        /// </summary>
        private void UpdateDescription()
        {
            this.DescriptionLabel.Text = ScheduleUtility.GetFullDescription(this.Schedule);
            this.ScheduleCalendar.Schedule = this.Schedule;
        }

        /// <summary>
        /// Updates the controls.
        /// </summary>
        private void UpdateControls()
        {
            this._isUpdating = true;
            this.UpdateMinuteControls();
            this.UpdateHourControls();
            this.UpdateDayControls();
            this.UpdateMonthControls();
            this.UpdateDayOfWeekControls();
            this._isUpdating = false;
        }

        /// <summary>
        /// Updates the minute controls.
        /// </summary>
        private void UpdateMinuteControls()
        {
            var description = ScheduleUtility.GetMinuteDescription(this.Schedule);
            if (description.IsEvery)
            {
                this.EveryMinuteRadioButton.IsChecked = true;
                if (string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    this.MinuteMultiSelect.SelectAll();
                }
                else
                {
                    SelectMultiSelectValues(this.MinuteMultiSelect, description);
                }
            }
            else
            {
                this.AtMinuteRadioButton.IsChecked = true;
                if (!string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    SelectMultiSelectValues(this.MinuteMultiSelect, description);
                }
            }
        }

        /// <summary>
        /// Updates the hours.
        /// </summary>
        private void UpdateHourControls()
        {
            var description = ScheduleUtility.GetHourDescription(this.Schedule);
            if (description.IsEvery)
            {
                this.EveryHourRadioButton.IsChecked = true;
                if (string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    this.HourMultiSelect.SelectAll();
                }
                else
                {
                    SelectMultiSelectValues(this.HourMultiSelect, description);
                }
            }
            else
            {
                this.AtHourRadioButton.IsChecked = true;
                if (!string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    SelectMultiSelectValues(this.HourMultiSelect, description);
                }
            }
        }

        /// <summary>
        /// Updates the days.
        /// </summary>
        private void UpdateDayControls()
        {
            var description = ScheduleUtility.GetDayDescription(this.Schedule);
            if (description.IsEvery)
            {
                this.EveryDayRadioButton.IsChecked = true;
                if (string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    this.DayMultiSelect.SelectAll();
                }
                else
                {
                    SelectMultiSelectValues(this.DayMultiSelect, description);
                }
            }
            else
            {
                this.AtDayRadioButton.IsChecked = true;
                if (!string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    SelectMultiSelectValues(this.DayMultiSelect, description);
                }
            }
        }

        /// <summary>
        /// Updates the months.
        /// </summary>
        private void UpdateMonthControls()
        {
            var description = ScheduleUtility.GetMonthDescription(this.Schedule);
            if (description.IsEvery)
            {
                this.EveryMonthRadioButton.IsChecked = true;
                if (string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    this.MonthMultiSelect.SelectAll();
                }
                else
                {
                    SelectMultiSelectValues(this.MonthMultiSelect, description);
                }
            }
            else
            {
                this.AtMonthRadioButton.IsChecked = true;
                if (!string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    SelectMultiSelectValues(this.MonthMultiSelect, description);
                }
            }
        }

        /// <summary>
        /// Updates the days of week.
        /// </summary>
        private void UpdateDayOfWeekControls()
        {
            var description = ScheduleUtility.GetDayOfWeekDescription(this.Schedule);
            IList<int> valuesToSelect;
            if (description.IsEvery)
            {
                if (string.IsNullOrWhiteSpace(description.PartialDescription))
                {
                    valuesToSelect = Enumerable.Range(0, 7).ToList();
                }
                else
                {
                    int repeatDay = description.ExpandedValues.First();
                    valuesToSelect = Enumerable.Range(0, 7).Where(i => i % repeatDay == 0).ToList();
                }
            }
            else
            {
                valuesToSelect = description.ExpandedValues;
            }

            this.SundayCheckBox.IsChecked = false;
            this.MondayCheckBox.IsChecked = false;
            this.TuesdayCheckBox.IsChecked = false;
            this.WednesdayCheckBox.IsChecked = false;
            this.ThursdayCheckBox.IsChecked = false;
            this.FridayCheckBox.IsChecked = false;
            this.SaturdayCheckBox.IsChecked = false;

            valuesToSelect.ToList().ForEach(i =>
            {
                this.SundayCheckBox.IsChecked |= i == 0;
                this.MondayCheckBox.IsChecked |= i == 1;
                this.TuesdayCheckBox.IsChecked |= i == 2;
                this.WednesdayCheckBox.IsChecked |= i == 3;
                this.ThursdayCheckBox.IsChecked |= i == 4;
                this.FridayCheckBox.IsChecked |= i == 5;
                this.SaturdayCheckBox.IsChecked |= i == 6;
            });
        }

        #endregion
    }
}
