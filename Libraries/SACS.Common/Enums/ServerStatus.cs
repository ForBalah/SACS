using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Enums
{
    /// <summary>
    /// The server status enum
    /// </summary>
    public enum ServerStatus
    {
        /// <summary>
        /// The status is unknown
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// The server is known to be in an error state
        /// </summary>
        Error = 1,

        /// <summary>
        /// The server has been started
        /// </summary>
        Started = 2,

        /// <summary>
        /// The server has been stopped.
        /// </summary>
        Stopped = 3,

        /// <summary>
        /// The server start is in progress.
        /// </summary>
        Starting = 4
    }
}
