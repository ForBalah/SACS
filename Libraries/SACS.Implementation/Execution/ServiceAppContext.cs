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
        /// Gets or sets a value indicating whether the context is executing.
        /// </summary>
        public bool IsExecuting { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether there was a failure during execution.
        /// </summary>
        public bool Failed { get; internal set; }

        /// <summary>
        /// Gets or sets the time that the context was created (and queued)
        /// </summary>
        public DateTime QueuedTime { get; internal set; }

        /// <summary>
        /// Gets or sets the ID that the current context uses.
        /// </summary>
        public int ContextId { get; internal set; }
    }
}