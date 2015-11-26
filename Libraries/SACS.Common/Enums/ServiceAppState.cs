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
        /// The service app is not loaded.
        /// </summary>
        [EnumMember]
        NotLoaded = 0,

        /// <summary>
        /// The service app is successfully initialized and ready to execute.
        /// </summary>
        [EnumMember]
        Ready = 1,

        /// <summary>
        /// The service app is currently executing.
        /// </summary>
        [EnumMember]
        Executing = 2,

        /// <summary>
        /// The service app errored while running.
        /// </summary>
        [EnumMember]
        Error = 3,

        /// <summary>
        /// The state of the service app (or non-service app) is unknown
        /// </summary>
        [EnumMember]
        Unknown = 4,
    }
}