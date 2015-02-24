using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Enums
{
    /// <summary>
    /// Represents the running state of the service app
    /// </summary>
    [DataContract]
    public enum ServiceAppState
    {
        /// <summary>
        /// The state is unknown.
        /// </summary>
        [EnumMember]
        Unknown = 0,

        /// <summary>
        /// The service app is not loaded
        /// </summary>
        [EnumMember]
        NotLoaded = 1,

        /// <summary>
        /// The service app is successfully initialized
        /// </summary>
        [EnumMember]
        Initialized = 2,

        /// <summary>
        /// The service app is currently executing
        /// </summary>
        [EnumMember]
        Executing = 3,

        /// <summary>
        /// The service app errored when starting
        /// </summary>
        [EnumMember]
        Error = 4,

        /// <summary>
        /// The service app is unloading
        /// </summary>
        [EnumMember]
        Unloading = 5
    }
}
