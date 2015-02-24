using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrontab;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The hour logic class
    /// </summary>
    internal class HourLogic : FieldLogic
    {
        /// <summary>
        /// The hour format
        /// </summary>
        private const string HourFormat = "{0:00}:{1:00}";

        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public override NCrontab.CrontabFieldKind FieldKind
        {
            get { return NCrontab.CrontabFieldKind.Hour; }
        }

        /// <summary>
        /// Gets the lower limit.
        /// </summary>
        /// <value>
        /// The lower limit.
        /// </value>
        public override int LowerLimit
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the upper limit.
        /// </summary>
        /// <value>
        /// The upper limit.
        /// </value>
        public override int UpperLimit
        {
            get { return 24; }
        }

        /// <summary>
        /// Gets the name of the qualifier (Usually "at", "on", or "in")
        /// </summary>
        /// <value>
        /// The name of the qualifier.
        /// </value>
        public override string SingularQualifierName
        {
            get
            {
                return "at";
            }
        }

        /// <summary>
        /// Builds the description for the specified type from the list of directives into the result StringBuilder
        /// </summary>
        /// <param name="directives">The directives.</param>
        /// <param name="result">The result StringBuilder.</param>
        /// <returns><c>true</c> if there should be a "skip" after building this description, otherwise <c>false</c>.</returns>
        public override bool BuildDescription(List<DescriptionDirective> directives, StringBuilder result)
        {
            DescriptionDirective directive = directives.FirstOrDefault(d => d.Field == this.FieldKind);
            if (directive != null)
            {
                if (directive.IsEvery)
                {
                    result.Append("every ");
                    result.Append(directive.PartialDescription);
                    result.Append(string.IsNullOrWhiteSpace(directive.PartialDescription) ? " hour " : " hours ");
                }
                else
                {
                    result.Append(this.SingularQualifierName + " ");

                    bool includeMinute = directives.Any(d => d.Field == CrontabFieldKind.Minute && !d.IsEvery);

                    if (includeMinute)
                    {
                        this.BuildSpecificTimes(directives, result);
                        return true;
                    }
                    else
                    {
                        this.BuildSpecificHours(directive, result);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Builds the specific hours.
        /// </summary>
        /// <param name="directive">The directive.</param>
        /// <param name="result">The result.</param>
        private void BuildSpecificHours(DescriptionDirective directive, StringBuilder result)
        {
            var parts = SplitValues(directive.PartialDescription);

            for (int i = 0; i < parts.Count; i++)
            {
                var hourPart = parts[i];
                result.Append(AddMeridian(hourPart.Item1));
                if (hourPart.Item2.HasValue)
                {
                    result.Append("-");
                    result.Append(AddMeridian(hourPart.Item2.Value));
                }

                if (i < parts.Count - 1)
                {
                    result.Append(",");
                }
            }
        }

        /// <summary>
        /// Builds the specific times in the format hh:mm
        /// </summary>
        /// <param name="directives">The directives.</param>
        /// <param name="result">The result.</param>
        private void BuildSpecificTimes(List<DescriptionDirective> directives, StringBuilder result)
        {
            DescriptionDirective hourDirective = directives.FirstOrDefault(d => d.Field == this.FieldKind);
            DescriptionDirective minuteDirective = directives.FirstOrDefault(d => d.Field == CrontabFieldKind.Minute);

            var hourParts = SplitValues(hourDirective.PartialDescription);
            var minuteParts = SplitValues(minuteDirective.PartialDescription);

            for (int h = 0; h < hourParts.Count; h++)
            {
                var hourPart = hourParts[h];
                AppendTimeWithMinutes(result, minuteParts, hourPart.Item1);

                if (hourPart.Item2.HasValue)
                {
                    result.Append("-");
                    AppendTimeWithMinutes(result, minuteParts, hourPart.Item2.Value);
                }

                if (h < hourParts.Count - 1)
                {
                    result.Append(",");
                }
            }
        }

        /// <summary>
        /// Appends the time with minutes.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="minuteParts">The minute parts.</param>
        /// <param name="hour">The hour.</param>
        private static void AppendTimeWithMinutes(StringBuilder result, IList<Tuple<int, int?>> minuteParts, int hour)
        {
            for (int m = 0; m < minuteParts.Count; m++)
            {
                var minutePart = minuteParts[m];
                result.Append(string.Format(HourFormat, hour, minutePart.Item1));
                if (minutePart.Item2.HasValue)
                {
                    result.Append("-");
                    result.Append(string.Format(HourFormat, hour, minutePart.Item2.Value));
                }

                if (m < minuteParts.Count - 1)
                {
                    result.Append(",");
                }
            }
        }

        /// <summary>
        /// Adds the meridian.
        /// </summary>
        /// <param name="hour">The hour.</param>
        /// <returns></returns>
        private static string AddMeridian(int hour)
        {
            if (hour == 12)
            {
                return "12pm";
            }
            else if (hour == 0)
            {
                return "midnight";
            }

            return hour < 12 ? string.Format("{0}am", hour) : string.Format("{0}pm", hour - 12);
        }
    }
}
