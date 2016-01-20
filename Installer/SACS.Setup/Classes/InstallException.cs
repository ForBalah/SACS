using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Classes
{
    public class InstallException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InstallException(string message)
            : base(message)
        {
        }
    }
}