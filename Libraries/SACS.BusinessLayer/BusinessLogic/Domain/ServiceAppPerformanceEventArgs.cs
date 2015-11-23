using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Contains service app performance event data
    /// </summary>
    public class ServiceAppPerformanceEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppPerformanceEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="failed">if set to <c>true</c> indicates the execution failed.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="message">The message.</param>
        public ServiceAppPerformanceEventArgs(string name, string guid, bool failed, DateTime? startTime, DateTime? endTime, string message)
        {
            this.Name = name;
            this.AppPerformance = new AppPerformance
            {
                Guid = guid,
                StartTime = startTime ?? default(DateTime),
                EndTime = endTime,
                Message = message,
                Failed = failed
            };
        }

        /// <summary>
        /// Gets the name of the service app
        /// </summary>
        /// <value>
        /// The name of the service app
        /// </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the application performance data
        /// </summary>
        /// <value>
        /// The application performance.
        /// </value>
        public AppPerformance AppPerformance { get; private set; }
    }
}
