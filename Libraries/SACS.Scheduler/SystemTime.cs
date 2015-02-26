using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Scheduler
{
    /// <summary>
    /// Provides the ability to override the current moment in time to facilitate testing.
    /// </summary>
    public static class SystemTime
    {
        /// <summary>
        /// Gets or sets the callback to be used to resolve the current moment in time.
        /// </summary>
        public static Func<DateTime> Resolver { get; set; }

        /// <summary>
        /// Gets the current moment in time.
        /// </summary>
        public static DateTime UtcNow
        {
            get { return Resolver == null ? DateTime.UtcNow : Resolver(); }
        }
    }
}
