using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="ex">The ex.</param>
        /// <param name="name">The name.</param>
        public ServiceAppErrorEventArgs(Exception ex, string name)
        {
            this.Exception = ex;
            this.Name = name;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
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
