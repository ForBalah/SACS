using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SACS.Implementation.Execution
{
    /// <summary>
    /// Manages the execution context for the service app. This class cannot be inherited.
    /// </summary>
    public sealed class ServiceAppContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppContext" /> class.
        /// </summary>
        internal ServiceAppContext()
        {
        }

        /// <summary>
        /// Gets a value indicating whether the context is executing.
        /// </summary>
        public bool IsExecuting { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether there was a failure during execution.
        /// </summary>
        public bool Failed { get; internal set; }

        /// <summary>
        /// Gets the time that the context was created (and queued)
        /// </summary>
        public DateTime QueuedTime { get; internal set; }

        /// <summary>
        /// Gets the ID that the current context uses.
        /// </summary>
        public string Guid { get; internal set; }

        /// <summary>
        /// Gets the current context's start execution time.
        /// </summary>
        public DateTime? StartTime { get; internal set; }

        /// <summary>
        /// Gets the current context's end execution time
        /// </summary>
        public DateTime EndTime { get; internal set; }
    }
}