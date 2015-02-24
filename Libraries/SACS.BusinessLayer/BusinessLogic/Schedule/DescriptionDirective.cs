using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Schedule
{
    /// <summary>
    /// Represents an intermediate step in translating the a crontab to natural language
    /// </summary>
    public class DescriptionDirective
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionDirective"/> class.
        /// </summary>
        internal DescriptionDirective()
        {
        }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        internal NCrontab.CrontabFieldKind Field { get; set; }

        /// <summary>
        /// Gets or sets the partial description.
        /// </summary>
        /// <value>
        /// The partial description.
        /// </value>
        public string PartialDescription { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the annotation is "every".
        /// </summary>
        /// <value>
        ///   <c>true</c> if the annotation is "every"; otherwise, <c>false</c>.
        /// </value>
        public bool IsEvery { get; set; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IList<Tuple<int, int?>> Values
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.PartialDescription))
                {
                    return FieldLogic.SplitValues(this.PartialDescription);
                }

                return new List<Tuple<int, int?>>();
            }
        }

        /// <summary>
        /// Gets the expanded values.
        /// </summary>
        /// <value>
        /// The expanded values.
        /// </value>
        public IList<int> ExpandedValues
        {
            get
            {
                List<int> valuesToSelect = new List<int>();
                foreach (var value in this.Values)
                {
                    valuesToSelect.Add(value.Item1);
                    if (value.Item2.HasValue)
                    {
                        Enumerable.Range(value.Item1 + 1, value.Item2.Value - value.Item1).ToList().ForEach(i =>
                        {
                            valuesToSelect.Add(i);
                        });
                    }
                }

                return valuesToSelect;
            }
        }
    }
}
