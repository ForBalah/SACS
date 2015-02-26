using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The day logic class
    /// </summary>
    internal class DayLogic : FieldLogic
    {
        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public override NCrontab.CrontabFieldKind FieldKind
        {
            get { return NCrontab.CrontabFieldKind.Day; }
        }

        /// <summary>
        /// Gets the lower limit.
        /// </summary>
        /// <value>
        /// The lower limit.
        /// </value>
        public override int LowerLimit
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets the upper limit.
        /// </summary>
        /// <value>
        /// The upper limit.
        /// </value>
        public override int UpperLimit
        {
            get { return 32; }
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
                return true;
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
                return "on the";
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
                    result.Append(string.IsNullOrWhiteSpace(directive.PartialDescription) ? " day " : " days ");
                }
                else
                {
                    result.Append(this.SingularQualifierName + " ");
                    var dayParts = SplitValues(directive.PartialDescription);

                    for (int i = 0; i < dayParts.Count; i++)
                    {
                        var dayPart = dayParts[i];
                        result.Append(this.MakethTheNumber(dayPart.Item1));
                        if (dayPart.Item2.HasValue)
                        {
                            result.Append("-");
                            result.Append(this.MakethTheNumber(dayPart.Item2.Value));
                        }

                        if (i < dayParts.Count - 1)
                        {
                            result.Append(",");
                        }
                    }
                    
                    result.Append(dayParts.Count == 1 ? " day " : " days ");
                }
            }

            return false;
        }

        /// <summary>
        /// Makethes the number. Yes, maketh it! I.e. -st, -nd, -rd
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        /// <remarks>
        /// There's a word for it I just don't know it.
        /// </remarks>
        private string MakethTheNumber(int number)
        {
            if (number % 10 == 1 && number != 11)
            {
                return number.ToString() + "st";
            }

            if (number % 10 == 2 && number != 12)
            {
                return number.ToString() + "nd";
            }

            if (number % 10 == 3 && number != 13)
            {
                return number.ToString() + "rd";
            }

            return number + "th";
        }
    }
}
