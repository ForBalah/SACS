using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Extensions
{
    /// <summary>
    /// Extension methods for the IEnumerable/Enumerable class
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns a list of indices where data points must be retained, inclusive of the first and
        /// last index.
        /// </summary>
        /// <param name="collection">The collection to get the list of indicies on.</param>
        /// <param name="maxPoints">The max points parameter.</param>
        /// <returns></returns>
        /// <remarks>The first and last indices are included in the final result. Hence "maxPoints"
        /// is inclusive of those points.</remarks>
        public static IEnumerable<int> GetIntervals<T>(this IEnumerable<T> collection, int maxPoints)
        {
            if (maxPoints < 0)
            {
                throw new ArgumentException("maxPoints must be zero or greater.");
            }

            var intervals = new List<int>();
            int count = collection.Count();
            if (count == 0)
            {
                return new List<int>();
            }
            else if (maxPoints == 0 || maxPoints > count)
            {
                return Enumerable.Range(0, count).ToList();
            }
            else if (maxPoints <= 2)
            {
                intervals.Add(0);
                intervals.Add(collection.Count() - 1);
                intervals = intervals.Distinct().ToList();
            }
            else
            {
                double realInterval = count / (double)(maxPoints - 1);
                double realIndex = 0;
                int lastIndex;
                int nextIndex;

                while ((int)realIndex <= count)
                {
                    lastIndex = intervals.DefaultIfEmpty(-1).LastOrDefault();
                    nextIndex = (int)realIndex;
                    if (nextIndex >= count)
                    {
                        nextIndex = count - 1; // we need the adjustment
                    }

                    if (lastIndex != nextIndex)
                    {
                        intervals.Add(nextIndex);
                    }

                    realIndex = realIndex + realInterval;
                }
            }

            return intervals;
        }
    }
}