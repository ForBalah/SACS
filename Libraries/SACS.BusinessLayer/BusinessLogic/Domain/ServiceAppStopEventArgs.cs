using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Contains the service app Stopped event args
    /// </summary>
    public class ServiceAppStopEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppStopEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the service app.</param>
        /// <param name="hasError"><c>true</c> if the the event had an error, otherwise, false.</param>
        public ServiceAppStopEventArgs(string name, bool hasError)
        {
            Name = name;
            HasError = hasError;
        }

        /// <summary>
        /// Gets a value indicating whether this stop event contains an error.
        /// </summary>
        public bool HasError
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the service app.
        /// </summary>
        /// <value>
        /// The name of the service app.
        /// </value>
        public string Name
        {
            get;
            private set;
        }
    }
}
