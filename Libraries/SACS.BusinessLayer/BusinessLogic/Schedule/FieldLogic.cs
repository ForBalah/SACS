using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCrontab;
using SACS.Common.Helpers;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// The base class with logic pertaining to each field
    /// </summary>
    internal abstract class FieldLogic
    {
        #region Properties

        /// <summary>
        /// Gets the kind of the field.
        /// </summary>
        /// <value>
        /// The kind of the field.
        /// </value>
        public abstract CrontabFieldKind FieldKind { get; }
        
        /// <summary>
        /// Gets the lower limit.
        /// </summary>
        /// <value>
        /// The lower limit.
        /// </value>
        public abstract int LowerLimit { get; }

        /// <summary>
        /// Gets the upper limit.
        /// </summary>
        /// <value>
        /// The upper limit.
        /// </value>
        public abstract int UpperLimit { get; }

        /// <summary>
        /// Gets a value indicating whether the lower limit is inclusive in the "every" calculation
        /// </summary>
        /// <value>
        /// <c>true</c> if the lower limit is inclusive in the "every" calculation; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLowerLimitInclusiveInEvery
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
        public abstract string SingularQualifierName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the description for the specified type from the list of directives into the result StringBuilder
        /// </summary>
        /// <param name="directives">The directives.</param>
        /// <param name="result">The result StringBuilder.</param>
        /// <returns></returns>
        public abstract bool BuildDescription(List<DescriptionDirective> directives, StringBuilder result);

        /// <summary>
        /// Builds the schedule part.
        /// </summary>
        /// <param name="part">The minute part.</param>
        /// <param name="result">The schedule builder.</param>
        /// <returns>
        /// A message if the build was not successful
        /// </returns>
        public virtual string BuildSchedulePart(Tuple<IList<int>, bool> part, StringBuilder result)
        {
            // Item2 is it's "IsEvery"
            if (part.Item2)
            {
                if (part.Item1.Count > 1)
                {
                    return string.Format("\"Every\" {0} can only have all or one selection", this.FieldKind.GetName());
                }

                result.Append("*");

                int adjustedLowerLimit = this.IsLowerLimitInclusiveInEvery ? this.LowerLimit : this.LowerLimit + 1;
                if (part.Item1.Count == 1 && part.Item1.First() >= adjustedLowerLimit)
                {
                    result.Append("/" + part.Item1.First().ToString());
                }
            }
            else
            {
                if (!part.Item1.Any())
                {
                    return string.Format("{0} \"{1}\" must have at least one selection", this.FieldKind.GetName(), this.SingularQualifierName);
                }

                result.Append(string.Join(",", part.Item1));
            }

            result.Append(" ");
            return string.Empty; // no message
        }

        /// <summary>
        /// Builds the schedule part.
        /// </summary>
        /// <param name="partList">The part list.</param>
        /// <param name="result">The schedule builder.</param>
        /// <returns>
        /// A message if the build was not successful
        /// </returns>
        public virtual string BuildSchedulePart(IList<int> partList, StringBuilder result)
        {
            if (!partList.Any())
            {
                return string.Format("{1} \"{0}\" must have at least one selection", this.SingularQualifierName, this.FieldKind.GetName());
            }

            result.Append(string.Join(",", partList) + " ");
            return string.Empty; // no message
        }
        
        /// <summary>
        /// Compacts the specified schedule part.
        /// </summary>
        /// <param name="schedulePart">The schedule part.</param>
        /// <returns></returns>
        public virtual string Compact(string schedulePart)
        {
            if (string.IsNullOrWhiteSpace(schedulePart) || schedulePart.Contains("*") || schedulePart.Contains("-"))
            {
                return schedulePart;
            }

            string[] numbers = schedulePart.Split(',');
            int? difference = null;
            for (int i = 1; i < numbers.Length; i++)
            {
                int first;
                int second;

                if (int.TryParse(numbers[i - 1], out first) && int.TryParse(numbers[i], out second))
                {
                    if (difference.HasValue && difference != second - first)
                    {
                        break;
                    }

                    difference = second - first;
                }
                else
                {
                    difference = null;
                    break;
                }
            }

            if (difference.HasValue)
            {
                // numbers must have at least 2 valid elements to make it into here
                int first = int.Parse(numbers.First());
                int last = int.Parse(numbers.Last());

                if (first == this.LowerLimit)
                {
                    bool isFilled = this.UpperLimit - difference <= last ? true : false;
                    if (isFilled)
                    {
                        // a difference means all numbers are equally spaced
                        return "*/" + difference.ToString();
                    }
                }
            }

            return schedulePart;
        }

        /// <summary>
        /// Gets the description directive.
        /// </summary>
        /// <param name="schedulePart">The schedule part.</param>
        /// <returns></returns>
        public virtual DescriptionDirective GetDescriptionDirective(string schedulePart)
        {
            DescriptionDirective directive = new DescriptionDirective();
            string compactSchedulePart = this.Compact(schedulePart);
            directive.Field = this.FieldKind;
            if (compactSchedulePart.Contains("*"))
            {
                directive.IsEvery = true;
                var splitValues = compactSchedulePart.Split('/');
                if (splitValues.Length > 1)
                {
                    directive.PartialDescription = splitValues[1];
                }
            }
            else
            {
                directive.IsEvery = false;
                directive.PartialDescription = compactSchedulePart;
            }

            return directive;
        }

        /// <summary>
        /// Splits the values into a list that can be more easily processed
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static IList<Tuple<int, int?>> SplitValues(string input)
        {
            string[] parts = input.Split(',');
            List<Tuple<int, int?>> finalList = new List<Tuple<int, int?>>();
            for (int i = 0; i < parts.Length; i++)
            {
                string valuePart = parts[i];
                string[] subParts = valuePart.Split('-');
                if (subParts.Length == 2)
                {
                    int left;
                    int right;
                    if (int.TryParse(subParts[0], out left) && int.TryParse(subParts[1], out right))
                    {
                        finalList.Add(new Tuple<int, int?>(left, right));
                    }
                }
                else
                {
                    int value;
                    if (int.TryParse(valuePart, out value))
                    {
                        finalList.Add(new Tuple<int, int?>(value, null));
                    }
                }
            }

            return finalList;
        }

        #endregion
    }
}
