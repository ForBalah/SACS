using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.WindowsService.Common
{
    /// <summary>
    /// The constants used exclusively in the Windows service.
    /// </summary>
    internal sealed class Constants
    {
        /// <summary>
        /// The date format
        /// </summary>
        public const string DateFormat = "yyyyMMddHHmm";

        /// <summary>
        /// The service description
        /// </summary>
        public const string ServiceDescription = "ServiceDescription";

        /// <summary>
        /// The service display name
        /// </summary>
        public const string ServiceDisplayName = "ServiceDisplayName";

        /// <summary>
        /// The service name
        /// </summary>
        public const string ServiceName = "ServiceName";

        /// <summary>
        /// The Health monitor's schedule
        /// </summary>
        public const string MonitorSchedule = "Monitor.Schedule";
    }
}
