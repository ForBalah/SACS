using System;
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
        public abstract string SerializeAsDebug(string message);

        /// <summary>
        /// Serializes the message as an info message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        public abstract string SerializeAsInfo(string message);

        /// <summary>
        /// Serializes the message as a performance message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract string SerializeAsPerformance(ServiceAppContext context);

        /// <summary>
        /// Serializes the message as an error message.
        /// </summary>
        /// <param name="ex">The exception to serialize..</param>
        /// <returns></returns>
        public abstract string SerializeAsError(Exception ex);

        /// <summary>
        /// Serializes the enum as a state message.
        /// </summary>
        /// <param name="state">The state.</param>
        public abstract string SerializeAsState(Enums.State state);

        /// <summary>
        /// Serializes the message as a result message - this is context specific and is not processed by
        /// the log.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        public abstract string SerializeAsResult(string message);

        /// <summary>
        /// Serializes the version as a string.
        /// </summary>
        /// <param name="version">The version to serialize.</param>
        /// <returns></returns>
        public abstract string SerializeAsVersion(Version version);
    }
}