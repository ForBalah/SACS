using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation
{
    /// <summary>
    /// Event args for passing messages to a log event.
    /// </summary>
    [Serializable]
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public Message Message
        {
            get;
            set;
        }
    }
}
