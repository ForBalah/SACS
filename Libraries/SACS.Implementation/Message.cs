using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation
{
    /// <summary>
    /// The message wrapper class for sending messages to a log.
    /// </summary>
    [Serializable]
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        public Message(string source, string message)
        {
            this.Source = source;
            this.Value = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="exception">The exception.</param>
        public Message(string source, Exception exception)
        {
            this.Source = source;
            this.Value = exception;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source
        {
            get;
            private set;
        }
    }
}
