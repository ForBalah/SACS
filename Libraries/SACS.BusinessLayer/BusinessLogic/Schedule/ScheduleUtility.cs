using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NCrontab;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The utility class for dealing with schedules
    /// </summary>
    public class ScheduleUtility
    {
        /// <summary>
        /// The default crontab.
        /// </summary>
        public const string DefaultCrontab = "* * * * *";

        private static Dictionary<CrontabFieldKind, FieldLogic> FieldLogicMap = new Dictionary<CrontabFieldKind, FieldLogic>
        {
            { CrontabFieldKind.Minute, new MinuteLogic() },
            { CrontabFieldKind.Hour, new HourLogic() },
            { CrontabFieldKind.Day, new DayLogic() },
            { CrontabFieldKind.Month, new MonthLogic() },
            { CrontabFieldKind.DayOfWeek, new DayOfWeekLogic() }
        };

        private static CrontabSchedule AnytimeSchedule = CrontabSchedule.Parse("* * * * *");
        
        /// <summary>
        /// The description representing any time
        /// </summary>
        public const string AnyTimeDescription = "Any time/every minute";

        /// <summary>
        /// Gets the next occurrences.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public static IList<DateTime> GetNextOccurrences(string crontab, DateTime from, DateTime to)
        {
            var ncrontab = CrontabSchedule.TryParse(crontab ?? string.Empty);
            if (ncrontab == null)
            {
                return new List<DateTime>();
            }

            return ncrontab.GetNextOccurrences(from, to).ToList();
        }

        /// <summary>
        /// Gets the full description of the cron tab schedule
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static string GetFullDescription(string crontab)
        {
            var ncrontab = CrontabSchedule.TryParse(crontab);
            if (ncrontab == null)
            {
                return string.Empty;
            }

            return BuildDescription(ncrontab);
        }

        /// <summary>
        /// Gets the minute values.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static DescriptionDirective GetMinuteDescription(string crontab)
        {
            string[] components = crontab.Split(' ');
            return FieldLogicMap[CrontabFieldKind.Minute].GetDescriptionDirective(components[0]);
        }

        /// <summary>
        /// Gets the hour values.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static DescriptionDirective GetHourDescription(string crontab)
        {
            string[] components = crontab.Split(' ');
            return FieldLogicMap[CrontabFieldKind.Hour].GetDescriptionDirective(components[1]);
        }

        /// <summary>
        /// Gets the day values.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static DescriptionDirective GetDayDescription(string crontab)
        {
            string[] components = crontab.Split(' ');
            return FieldLogicMap[CrontabFieldKind.Day].GetDescriptionDirective(components[2]);
        }

        /// <summary>
        /// Gets the month description.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static DescriptionDirective GetMonthDescription(string crontab)
        {
            string[] components = crontab.Split(' ');
            return FieldLogicMap[CrontabFieldKind.Month].GetDescriptionDirective(components[3]);
        }

        /// <summary>
        /// Gets the day of week description.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        public static DescriptionDirective GetDayOfWeekDescription(string crontab)
        {
            string[] components = crontab.Split(' ');
            return FieldLogicMap[CrontabFieldKind.DayOfWeek].GetDescriptionDirective(components[4]);
        }

        /// <summary>
        /// Builds the schedule.
        /// </summary>
        /// <param name="minutePart">The minute part.</param>
        /// <param name="hourPart">The hour part.</param>
        /// <param name="dayPart">The day part.</param>
        /// <param name="monthPart">The month part.</param>
        /// <param name="dayOfWeekPart">The day of week part.</param>
        /// <returns></returns>
        public static ScheduleBuildResult BuildSchedule(Tuple<IList<int>, bool> minutePart, Tuple<IList<int>, bool> hourPart, Tuple<IList<int>, bool> dayPart, Tuple<IList<int>, bool> monthPart, IList<int> dayOfWeekPart)
        {
            ScheduleBuildResult result = new ScheduleBuildResult();
            StringBuilder scheduleBuilder = new StringBuilder();

            BuildSchedulePart(minutePart, CrontabFieldKind.Minute, scheduleBuilder, result);
            BuildSchedulePart(hourPart, CrontabFieldKind.Hour, scheduleBuilder, result);
            BuildSchedulePart(dayPart, CrontabFieldKind.Day, scheduleBuilder, result);
            BuildSchedulePart(monthPart, CrontabFieldKind.Month, scheduleBuilder, result);
            BuildSchedulePart(dayOfWeekPart, CrontabFieldKind.DayOfWeek, scheduleBuilder, result);

            result.Result = scheduleBuilder.ToString().Trim();
            return result;
        }

        /// <summary>
        /// Builds the schedule part.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="crontabFieldKind">Kind of the crontab field.</param>
        /// <param name="scheduleBuilder">The schedule builder.</param>
        /// <param name="result">The result.</param>
        private static void BuildSchedulePart(Tuple<IList<int>, bool> part, CrontabFieldKind crontabFieldKind, StringBuilder scheduleBuilder, ScheduleBuildResult result)
        {
            string message = FieldLogicMap[crontabFieldKind].BuildSchedulePart(part, scheduleBuilder);
            if (!string.IsNullOrWhiteSpace(message))
            {
                result.Messages.Add(message);
            }
        }

        /// <summary>
        /// Builds the schedule part.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="crontabFieldKind">Kind of the crontab field.</param>
        /// <param name="scheduleBuilder">The schedule builder.</param>
        /// <param name="result">The result.</param>
        private static void BuildSchedulePart(IList<int> part, CrontabFieldKind crontabFieldKind, StringBuilder scheduleBuilder, ScheduleBuildResult result)
        {
            string message = FieldLogicMap[crontabFieldKind].BuildSchedulePart(part, scheduleBuilder);
            if (!string.IsNullOrWhiteSpace(message))
            {
                result.Messages.Add(message);
            }
        }

        /// <summary>
        /// Consolidates the directives.
        /// </summary>
        /// <param name="directives">The directives.</param>
        internal static void ConsolidateDirectives(List<DescriptionDirective> directives)
        {
            // starting from the end, work backwards, eliminating any directives that represent every time in that component
            for (int i = directives.Count - 1; i > 0; i--)
            {
                if (directives[i].IsEvery &&
                    string.IsNullOrWhiteSpace(directives[i].PartialDescription) &&
                    directives[i - 1].IsEvery)
                {
                    directives.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Gets the list of directives.
        /// </summary>
        /// <param name="crontab">The crontab.</param>
        /// <returns></returns>
        internal static List<DescriptionDirective> GetDirectives(string crontab)
        {
            List<DescriptionDirective> directives = new List<DescriptionDirective>();
            directives.Add(GetMinuteDescription(crontab));
            directives.Add(GetHourDescription(crontab));
            directives.Add(GetDayDescription(crontab));
            directives.Add(GetMonthDescription(crontab));
            directives.Add(GetDayOfWeekDescription(crontab));

            return directives;
        }

        /// <summary>
        /// Builds the description.
        /// </summary>
        /// <param name="ncrontab">The ncrontab.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static string BuildDescription(CrontabSchedule ncrontab)
        {
            if (ncrontab.ToString() == AnytimeSchedule.ToString())
            {
                return AnyTimeDescription;
            }

            StringBuilder result = new StringBuilder();
            List<DescriptionDirective> directives = GetDirectives(ncrontab.ToString());
            ConsolidateDirectives(directives);

            // build the final description backwards
            for (int i = directives.Count - 1; i >= 0; i--)
            {
                bool skipNext = FieldLogicMap[directives[i].Field].BuildDescription(directives, result);

                if (!skipNext && i > 0)
                {
                    result.Append(", ");
                }

                if (skipNext)
                {
                    i--;
                }
            }

            return CleanDescription(result);
        }

        /// <summary>
        /// Cleans the description, making title case, remove extra spaces etc.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private static string CleanDescription(StringBuilder result)
        {
            if (result.Length == 0)
            {
                return string.Empty;
            }
            
            // Fix title case
            result[0] = result[0].ToString().ToUpper()[0];
            string final = result.ToString();
            final = Regex.Replace(final, @"\s+", " ");
            final = Regex.Replace(final, @"\s+,", ",");

            return final.Trim();
        }
    }
}
