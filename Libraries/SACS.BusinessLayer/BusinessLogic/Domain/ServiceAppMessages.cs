using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.Implementation;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Class that holds all the various types of messages that a service app can give
    /// </summary>
    public sealed class ServiceAppMessages
    {
        /// <summary>
        /// ServiceApp successfully initialized
        /// </summary>
        public const string SuccesfulInitialization = "Service has been initialized successfully.";

        /// <summary>
        /// ServiceApp initializeed with an error
        /// </summary>
        public const string InitializedWithError = "Service has been initialized but has an error.";

        /// <summary>
        /// ServiceApp failed to initialize
        /// </summary>
        public const string FailedToInitialize = "Service has failed to initialized.";

        /// <summary>
        /// ServiceApp has an invalid schedule
        /// </summary>
        public const string InvalidSchedule = "Service has an invalid schedule";

        /// <summary>
        /// Service has successfully been unloaded
        /// </summary>
        public const string SuccessfulUnload = "Service has successfully been unloaded";

        /// <summary>
        /// Service is being unloaded
        /// </summary>
        public const string Unloading = "Service is being unloaded";
    }
}
