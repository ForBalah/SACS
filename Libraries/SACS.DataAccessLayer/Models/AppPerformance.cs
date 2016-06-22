using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// The app performance class.  In hindsight this should have been in a separate models project.
    /// </summary>
    public class AppPerformance
    {
        private const string DateFormat = "yyyy/MM/dd HH:mm:ss.fff";

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Obsolete("Dropped in favour of Guid")]
        public int Identifier { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets the start time string equivalent.
        /// </summary>
        /// <value>
        /// The start time string.
        /// </value>
        public string StartTimeString
        {
            get
            {
                return this.StartTime.ToString(DateFormat);
            }
        }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets the end time string equivalent.
        /// </summary>
        /// <value>
        /// The end time string.
        /// </value>
        public string EndTimeString
        {
            get
            {
                if (this.EndTime.HasValue)
                {
                    return this.EndTime.Value.ToString(DateFormat);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the duration, in seconds.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public double Duration
        {
            get
            {
                DateTime endTime = this.EndTime ?? this.StartTime;
                return (double)(endTime - this.StartTime).Milliseconds / 1000d;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the performance execution was a failure.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the performance execution was a failure; otherwise, <c>false</c>.
        /// </value>
        public bool Failed { get; set; }
    }
}
