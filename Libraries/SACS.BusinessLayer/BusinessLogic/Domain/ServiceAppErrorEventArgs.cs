using System;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Contains service app error event data
    /// </summary>
    public class ServiceAppErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppErrorEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception associated with the event.</param>
        /// <param name="name">The name of the service app.</param>
        public ServiceAppErrorEventArgs(Exception ex, string name)
        {
            Exception = ex;
            Name = name;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception
        {
            get;
            private set;
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
    }
}
