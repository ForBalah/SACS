using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Extensions
{
    /// <summary>
    /// The DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Determines whether a subject date is the same as a date passed in.
        /// </summary>
        /// <param name="subjectDate"> The subject date.</param>
        /// <param name="dateToCompare">The date passed in.</param>
        /// <returns>True if the two DateTime objects represent the same date, even if their time components differ; false otherwise.</returns>
        public static bool IsSameDateAs(this DateTime subjectDate, DateTime dateToCompare)
        {
            var dayIsSame = subjectDate.Day == dateToCompare.Day;
            var monthIsSame = subjectDate.Month == dateToCompare.Month;
            var yearIsSame = subjectDate.Year == dateToCompare.Year;
            return dayIsSame && monthIsSame && yearIsSame;
        }

        /// <summary>
        /// Determines whether a subject date is in the same month as a date passed in.
        /// </summary>
        /// <param name="subjectDate"> The subject date.</param>
        /// <param name="dateToCompare">The date passed in.</param>
        /// <returns>True if the two DateTime objects are in the same month; false otherwise.</returns>
        public static bool IsSameMonthAs(this DateTime subjectDate, DateTime dateToCompare)
        {
            var monthIsSame = subjectDate.Month == dateToCompare.Month;
            var yearIsSame = subjectDate.Year == dateToCompare.Year;
            return monthIsSame && yearIsSame;
        }

        /// <summary>
        /// Gets the last day of the month for the give date.
        /// </summary>
        /// <param name="subjectDate">The subject date.</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime subjectDate)
        {
            var addedMonth = subjectDate.AddMonths(1);
            return new DateTime(addedMonth.Year, addedMonth.Month, 1).AddDays(-1);
        }
    }
}
