using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// The system performance model.
    /// </summary>
    public class SystemPerformance
    {
        /// <summary>
        /// Gets or sets the audit time.
        /// </summary>
        /// <value>
        /// The audit time.
        /// </value>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Value { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            SystemPerformance b = obj as SystemPerformance;
            return this.AuditTime.Equals(b.AuditTime) &&
                this.Value.Equals(b.Value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.AuditTime.GetHashCode() ^ this.Value.GetHashCode();
        }

        /// <summary>
        /// Compacts the data, to make it easier to render out
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="secondsInterval">The seconds interval.</param>
        public static void CompactData(IList<SystemPerformance> data, int secondsInterval)
        {
            Debug.Assert(secondsInterval >= 0, "secondsInterval cannot be less than zero");

            bool keepSameValue = false;
            for (var i = 0; i < data.Count; i++)
            {
                SystemPerformance prevDataPoint = i > 0 ? data[i - 1] : null;
                SystemPerformance dataPoint = data[i];
                SystemPerformance nextDataPoint = i < data.Count - 1 ? data[i + 1] : null;

                bool areAllTheSame = prevDataPoint != null &&
                    nextDataPoint != null &&
                    prevDataPoint.Value == dataPoint.Value &&
                    dataPoint.Value == nextDataPoint.Value;

                if (prevDataPoint != null &&
                    (dataPoint.AuditTime - prevDataPoint.AuditTime).TotalSeconds > secondsInterval &&
                    dataPoint.Value != 0 &&
                    (!keepSameValue || prevDataPoint.Value != dataPoint.Value))
                {
                    // if there is a greater-than-interval time difference between the previous and
                    // current data points, insert zero record at current position (to push the rest out by one)
                    data.Insert(i, new SystemPerformance { AuditTime = dataPoint.AuditTime.AddSeconds(-1), Value = 0 });
                    keepSameValue = false;
                }
                else if (nextDataPoint != null &&
                    (nextDataPoint.AuditTime - dataPoint.AuditTime).TotalSeconds > secondsInterval &&
                    dataPoint.Value != 0 &&
                    (!keepSameValue || dataPoint.Value != nextDataPoint.Value))
                {
                    // if there is a greater-than-interval time difference between the current and
                    // next data points, insert zero record at current position + 1 (to be processed next pass)
                    data.Insert(i + 1, new SystemPerformance { AuditTime = dataPoint.AuditTime.AddSeconds(1), Value = 0 });
                    keepSameValue = false;
                }
                else if (areAllTheSame)
                {
                    // if the data is the same in all instances, remove it
                    data.RemoveAt(i);
                    keepSameValue = true;
                    i--; // adjust i
                }
            }
        }
    }
}
