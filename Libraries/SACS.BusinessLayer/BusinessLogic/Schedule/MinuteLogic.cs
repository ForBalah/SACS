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
    internal class MinuteLogic : FieldLogic
    {
        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public override NCrontab.CrontabFieldKind FieldKind
        {
            get { return NCrontab.CrontabFieldKind.Minute; }
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
            get { return 60; }
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
                result.Append(directive.IsEvery ? "every " : this.SingularQualifierName + " ");
                result.Append(directive.PartialDescription);
                result.Append(string.IsNullOrWhiteSpace(directive.PartialDescription) ? " minute " : " minutes ");
            }

            return false;
        }
    }
}
