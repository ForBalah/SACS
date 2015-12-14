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
        #region Properties

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

        #endregion Properties

        #region Methods

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

                bool areAllTheSame = dataPoint.IsTheSameAs(prevDataPoint, nextDataPoint);

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

        /// <summary>
        /// Lowers the resolution of the performance data to save rendering time
        /// </summary>
        /// <param name="data">The data to lower the resolution on.</param>
        /// <param name="maxPoints">The maximum number of points to keep.</param>
        /// <param name="threshold">The threshold at which to show data points regardless of resolution.</param>
        public static void LowerResolution(IList<SystemPerformance> data, int maxPoints, decimal threshold)
        {
            Debug.Assert(maxPoints >= 0, "maxPoints cannot be less than zero");
            Debug.Assert(threshold >= 0, "maxPoints threshold be less than zero");

            var intervals = GetIntervals(data.Count, maxPoints);

            for (var i = 0; i < data.Count; i++)
            {
                bool isIntervalPoint = intervals.Any(a => i == a);
                bool removePoint = false;
                decimal prevValue = i > 0 ? data[i - 1].Value : 0;
                decimal currentValue = data[i].Value;
                decimal nextValue = i < data.Count - 1 ? data[i + 1].Value : 0;

                // keeping points that are either on the interval or outside of the threshold
                // will ensure that we keep seeing accurate data.
                bool isResolutionPoint = i == 0 || i == data.Count - 1 || isIntervalPoint;

                if (!isResolutionPoint)
                {
                    decimal prevDelta = SafeDivide(Math.Abs(currentValue - prevValue), currentValue);
                    decimal nextDelta = SafeDivide(Math.Abs(currentValue - nextValue), currentValue);

                    if (prevDelta < threshold && nextDelta < threshold)
                    {
                        removePoint = true;
                    }
                }

                if (removePoint)
                {
                    data.RemoveAt(i);
                    i--;
                    for (int j = 0; j < intervals.Count; j++)
                    {
                        intervals[j]--;
                    }
                }
            }
        }

        /// <summary>
        /// Determines of the system performance points are all the same
        /// </summary>
        /// <param name="prevDataPoint">The previous data point to compare to.</param>
        /// <param name="nextDataPoint">The next data point to compare to.</param>
        /// <returns></returns>
        public bool IsTheSameAs(SystemPerformance prevDataPoint, SystemPerformance nextDataPoint)
        {
            bool areAllTheSame = prevDataPoint != null &&
                nextDataPoint != null &&
                prevDataPoint.Value == this.Value &&
                this.Value == nextDataPoint.Value;
            return areAllTheSame;
        }

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
        /// Returns a list of indices where the data points must be retained.
        /// </summary>
        /// <param name="count">The list count.</param>
        /// <param name="maxPoints">The max points parameter</param>
        /// <returns></returns>
        private static IList<int> GetIntervals(int count, int maxPoints)
        {
            if (count == 0)
            {
                return new List<int>();
            }

            if (maxPoints == 0)
            {
                return Enumerable.Range(0, count).ToList();
            }

            var intervals = new List<int>();
            int lastPoint = 0;

            while (maxPoints > intervals.Count)
            {
                lastPoint = ((count - lastPoint) / (maxPoints + 1)) + lastPoint;
                intervals.Add(lastPoint);
                lastPoint++;
            }

            return intervals;
        }

        /// <summary>
        /// Performs a safe division, returning 1 where the denominator is zero.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        private static decimal SafeDivide(decimal numerator, decimal denominator)
        {
            if (denominator == 0)
            {
                return 1;
            }

            return numerator / denominator;
        }

        #endregion Methods
    }
}