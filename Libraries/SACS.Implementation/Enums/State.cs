using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Enums
{
    /// <summary>
    /// Represents the state of the ServiceApp
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Service App state is unknown
        /// </summary>
        [Description("Unkown")]
        Unknown = 0,

        /// <summary>
        /// The service app is started but has not yet been executed for the first time.
        /// </summary>
        [Description("Started")]
        Started = 1,

        /// <summary>
        /// The service app is executing
        /// </summary>
        [Description("Executing")]
        Executing = 2,

        /// <summary>
        /// The service app is idle
        /// </summary>
        [Description("Idle")]
        Idle = 3,

        /// <summary>
        /// The service app is stopped
        /// </summary>
        [Description("Stopped")]
        Stopped = 4,

        /// <summary>
        /// The service app has failed
        /// </summary>
        [Description("Failed")]
        Failed = 5
    }
}
