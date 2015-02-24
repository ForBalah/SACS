using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The minute logic class
    /// </summary>
    internal class DayOfWeekLogic : FieldLogic
    {
        private static List<string> DayOfWeek = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public override NCrontab.CrontabFieldKind FieldKind
        {
            get { return NCrontab.CrontabFieldKind.DayOfWeek; }
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
            get { return 7; }
        }

        /// <summary>
        /// Gets a value indicating whether the lower limit is inclusive in the "every" calculation
        /// </summary>
        /// <value>
        /// <c>true</c> if the lower limit is inclusive in the "every" calculation; otherwise, <c>false</c>.
        /// </value>
        public override bool IsLowerLimitInclusiveInEvery
        {
            get
            {
                return false;
            }
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
                return "on";
            }
        }

        /// <summary>
        /// Compacts the specified schedule part.
        /// </summary>
        /// <param name="schedulePart">The schedule part.</param>
        /// <returns></returns>
        public override string Compact(string schedulePart)
        {
            // We choose not to compact the day of the week as we want it to be written out in full
            return schedulePart;
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
                // cron should not be compacted here. so * should not have any number with it
                if (directive.IsEvery)
                {
                    result.Append("any day of the week ");
                }
                else
                {
                    result.Append(this.SingularQualifierName + " ");
                    var dayParts = SplitValues(directive.PartialDescription);

                    for (int i = 0; i < dayParts.Count; i++)
                    {
                        var dayPart = dayParts[i];
                        result.Append(DayOfWeek[dayPart.Item1]);
                        if (dayPart.Item2.HasValue)
                        {
                            result.Append("-");
                            result.Append(DayOfWeek[dayPart.Item2.Value]);
                        }

                        if (i < dayParts.Count - 1)
                        {
                            result.Append(",");
                        }
                    }
                }
            }

            return false;
        }
    }
}
