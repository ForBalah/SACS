using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// The metrics summarizing class for app metrics
    /// </summary>
    [Obsolete("Different metrics system will be used.")]
    public class AppMetrics
    {
        /*
        /// <summary>
        /// Gets the average execution.
        /// </summary>
        /// <value>
        /// The average execution.
        /// </value>
        public double AverageExecution { get; }

        public double FastestExecution { get; }

        public double SlowestExecution { get; }

        public DateTime FastestExecutionDate { get; }

        public DateTime SlowestExecutionDate { get; }

        public int ExecutionCount { get; }

        public int ErrorCount { get; }

        public double ExecutionErrorRatio { get; }

        /// <summary>
        /// Updates the metrics using the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public bool UpdateMetrics(IEnumerable<AppPerformance> data)
        {
            throw new NotImplementedException();
        }
        */
    }
}
