using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation.Execution
{
    /// <summary>
    /// Represents the type of execution capability that the service app has.
    /// </summary>
    public enum ExecutionMode
    {
        /// <summary>
        /// Default execution mode, which is equivalent of Idempotent
        /// </summary>
        Default = 0,

        /// <summary>
        /// Indicates that if there are multiple execution invocations that happen simultaneously,
        /// only the first invocation is recorded.
        /// </summary>
        Idempotent = 1,

        /// <summary>
        /// Indicates that if there are multiple execution invocations that happen simultaneously,
        /// each will be deferred until the end of the previous invocation.
        /// </summary>
        Inline = 2,

        /// <summary>
        /// Indicates that if there are multiple execution invocations that happen simultaneously,
        /// each will be occur as requested.
        /// </summary>
        Concurrent = 3
    }
}