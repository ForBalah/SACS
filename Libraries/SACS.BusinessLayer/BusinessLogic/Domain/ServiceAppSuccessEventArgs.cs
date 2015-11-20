using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Contains the service app success event data
    /// </summary>
    public class ServiceAppSuccessEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppSuccessEventArgs"/> class.
        /// </summary>
        /// <param name="serviceApp">The service app to create the success data from.</param>
        /// <param name="finishTime">The time the service app finished executing.</param>
        public ServiceAppSuccessEventArgs(ServiceApp serviceApp, DateTime finishTime)
        {
            this.Name = serviceApp.Name;
            this.SendSuccessNotification = serviceApp.SendSuccessNotification;
            this.Environment = serviceApp.Environment;
            this.FinishTime = finishTime;
        }

        /// <summary>
        /// Gets the name of the service app
        /// </summary>
        /// <value>
        /// The name of the service app
        /// </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether to send a successful execution notification.
        /// </summary>
        public bool SendSuccessNotification
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the time that the service app finished executing.
        /// </summary>
        public DateTime FinishTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the environment
        /// </summary>
        public string Environment
        {
            get;
            private set;
        }
    }
}