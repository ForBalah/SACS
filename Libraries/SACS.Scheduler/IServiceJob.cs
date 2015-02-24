using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Scheduler
{
    /// <summary>
    /// The service job interface
    /// </summary>
    public interface IServiceJob
    {
        /// <summary>
        /// Gets the name of this service job
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the next occurence function for this job
        /// </summary>
        Func<DateTime, DateTime> NextOccurence { get; }

        /// <summary>
        /// Gets a value indicating whether this job is currently executing. Note this is only reliable if the execution step is synchronous,
        /// otherwise this will most likely always be false
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// Executes this ServiceJob instance
        /// </summary>
        void Execute();
    }
}
