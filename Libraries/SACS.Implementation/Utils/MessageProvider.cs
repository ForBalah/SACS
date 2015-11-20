using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SACS.Implementation.Execution;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Adds the behaviour to processing messages.
    /// </summary>
    public abstract class MessageProvider
    {
        /// <summary>
        /// Serializes the message as a debug message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal abstract string SerializeAsDebug(string message);

        /// <summary>
        /// Serializes the message as an info message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal abstract string SerializeAsInfo(string message);

        /// <summary>
        /// Serializes the message as a performance message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal abstract string SerializeAsPerformance(ServiceAppContext context, string message);

        /// <summary>
        /// Serializes the message as an error message.
        /// </summary>
        /// <param name="ex">The exception to serialize..</param>
        /// <returns></returns>
        internal abstract string SerializeAsError(Exception ex);

        /// <summary>
        /// Serializes the enum as a state message.
        /// </summary>
        /// <param name="state">The state.</param>
        internal abstract string SerializeAsState(Enums.State state);

        /// <summary>
        /// Serializes the message as a result message - this is context specific and is not processed by
        /// the log.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal abstract string SerializeAsResult(string message);
    }
}