using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Enums
{
    /// <summary>
    /// The Log Entry image type, based on the log level
    /// </summary>
    public enum LogImageType
    {
        /// <summary>
        /// The debug
        /// </summary>
        Debug = 0,

        /// <summary>
        /// The error
        /// </summary>
        Error = 1,

        /// <summary>
        /// The fatal
        /// </summary>
        Fatal = 2,

        /// <summary>
        /// The information
        /// </summary>
        Info = 3,

        /// <summary>
        /// The warn
        /// </summary>
        Warn = 4,

        /// <summary>
        /// The custom
        /// </summary>
        Custom = 5
    }
}
