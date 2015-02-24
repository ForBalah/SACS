using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The month logic class
    /// </summary>
    internal class MonthLogic : FieldLogic
    {
        private static List<string> Month = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public override NCrontab.CrontabFieldKind FieldKind
        {
            get { return NCrontab.CrontabFieldKind.Month; }
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
            get { return 13; }
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
                return "in";
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
                    result.Append(string.IsNullOrWhiteSpace(directive.PartialDescription) ? " month " : " months ");
                }
                else
                {
                    result.Append(this.SingularQualifierName + " ");
                    var monthParts = SplitValues(directive.PartialDescription);

                    for (int i = 0; i < monthParts.Count; i++)
                    {
                        var montPart = monthParts[i];
                        result.Append(Month[montPart.Item1 - 1]);
                        if (montPart.Item2.HasValue)
                        {
                            result.Append("-");
                            result.Append(Month[montPart.Item2.Value - 1]);
                        }

                        if (i < monthParts.Count - 1)
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
