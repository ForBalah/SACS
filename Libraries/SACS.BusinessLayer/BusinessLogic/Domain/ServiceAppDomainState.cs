using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// The state of the ServiceAppDomain
    /// </summary>
    [Obsolete("Dropped in favour of ServiceAppState")]
    public enum ServiceAppDomainState
    {
        /// <summary>
        /// The ServiceAppDomain is not started and no attempt to start it
        /// </summary>
        NotInitialized = 0,

        /// <summary>
        /// The ServiceAppDomain has been initialized successfully
        /// </summary>
        Initialized = 1,

        /// <summary>
        /// The ServiceAppDomain was started, but errored out
        /// </summary>
        WithError = 2,

        /// <summary>
        /// The ServiceAppDomain is currently unloading
        /// </summary>
        Unloading = 3
    }
}
