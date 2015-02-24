using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Enums
{
    /// <summary>
    /// Enum for determining the start up type of the service app
    /// </summary>
    [DataContract]
    public enum StartupType
    {
        /// <summary>
        /// The startup type is not determined
        /// </summary>
        [EnumMember]
        NotSet = 0,

        /// <summary>
        /// The service will be manually started
        /// </summary>
        [EnumMember]
        Manual = 1,

        /// <summary>
        /// The service will start when the SAC starts
        /// </summary>
        [EnumMember]
        Automatic = 2,

        /// <summary>
        /// The service will not start automatically when the SAC starts
        /// </summary>
        [EnumMember]
        Disabled = 3
    }
}
