using System;
using System.Web.Script.Serialization;
using SACS.Implementation.Execution;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Adds JSON message processing capabilities.
    /// </summary>
    internal class JsonMessageProvider : MessageProvider
    {
        /// <summary>
        /// The serializer.
        /// </summary>
        /// <remarks>
        /// The in-built JavaScript serializer in .NET is used instead of the more
        /// popular JSON.NET libary because the intention is to make sure that only
        /// one dll (this one) is needed as a dependency.
        /// </remarks>
        private readonly JavaScriptSerializer serializer;

        #region Constructors and Destructors

        public JsonMessageProvider()
        {
            serializer = new JavaScriptSerializer();
        }

        #endregion Constructors and Destructors

        #region Methods

        /// <summary>
        /// Serializes the message as an info message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        public override string SerializeAsInfo(string message)
        {
            return serializer.Serialize(new { info = message });
        }

        /// <summary>
        /// Serializes the message as a performance message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override string SerializeAsPerformance(ServiceAppContext context)
        {
            return serializer.Serialize(new
            {
                performance = new
                {
                    name = context.Name,
                    guid = context.Guid,
                    startTime = context.StartTime,
                    endTime = context.EndTime,
                    failed = context.Failed,
                    message = context.CustomMessage
                }
            });
        }

        /// <summary>
        /// Serializes the message as a debug message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        public override string SerializeAsDebug(string message)
        {
            return serializer.Serialize(new { debug = message });
        }

        /// <summary>
        /// Serializes the message as an error message.
        /// </summary>
        /// <param name="ex">The exception to serialize.</param>
        /// <returns></returns>
        public override string SerializeAsError(Exception ex)
        {
            return serializer.Serialize(new
            {
                error = new
                {
                    details = new
                    {
                        type = ex.GetType().ToString(),
                        message = ex.Message,
                        source = ex.Source,
                        stackTrace = ex.StackTrace
                    },
                    exception = ex.ToBase64()
                }
            });
        }

        /// <summary>
        /// Serializes the enum as a state message.
        /// </summary>
        /// <param name="state">The state.</param>
        public override string SerializeAsState(Enums.State state)
        {
            return serializer.Serialize(new { state = Enum.GetName(typeof(Enums.State), state) });
        }

        /// <summary>
        /// Serializes the message as a result message - this is context specific and is not processed by
        /// the log.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        public override string SerializeAsResult(string message)
        {
            return serializer.Serialize(new { result = message });
        }

        /// <summary>
        /// Serializes the version as a string.
        /// </summary>
        /// <param name="version">The version to serialize.</param>
        /// <returns></returns>
        public override string SerializeAsVersion(Version version)
        {
            return serializer.Serialize(new { version = version.ToString() });
        }

        #endregion Methods
    }
}