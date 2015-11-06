using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets a value indicating whether the execution context can actually be executed on.
        /// </summary>
        /// <param name="context">The service app execution context to test.</param>
        /// <returns></returns>
        public bool CanExecute
        {
            get
            {
                return !this.Failed && !this.StartTime.HasValue;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the context is executing.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return this.StartTime.HasValue && !this.EndTime.HasValue;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there was a failure during execution.
        /// </summary>
        public bool Failed { get; internal set; }

        /// <summary>
        /// Gets the time that the context was created (and queued).
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
        /// Gets the current context's end execution time.
        /// </summary>
        public DateTime? EndTime { get; internal set; }

        /// <summary>
        /// Gets the name of the service app that this context is associated with.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets or sets the current context's execution handle.
        /// </summary>
        internal Task Handle { get; set; }

        /// <summary>
        /// Gets the duration of the execution.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration
        {
            get
            {
                if (this.StartTime.HasValue && this.EndTime.HasValue)
                {
                    return this.EndTime.Value.Subtract(this.StartTime.Value);
                }

                return new TimeSpan(0);
            }
        }
    }
}