using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.WindowsService.Enums
{
    /// <summary>
    /// The flags which represent service state. Note multiple states can co-exist
    /// </summary>
    internal enum ServiceState
    {
        /// <summary>
        /// Service state is unknown. this is the default
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The service is starting
        /// </summary>
        Running = 1 << 1,

        /// <summary>
        /// The Web conponent of the service is started
        /// </summary>
        StartedWebComponent = 1 << 2,

        /// <summary>
        /// The health monitor component is started
        /// </summary>
        StartedMonitorComponent = 1 << 3,

        /// <summary>
        /// The windows component is started
        /// </summary>
        StartedWindowsComponent = 1 << 4
    }
}
