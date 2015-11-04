using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using SACS.Implementation.Execution;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Adds JSON message processing capabilities
    /// </summary>
    internal class JsonMessageProvider : MessageProvider
    {
        /// <summary>
        /// The serializer
        /// </summary>
        /// <remarks>
        /// The in-built JavaScript serializer in .NET is used instead of the more 
        /// popular JSON.NET libary because the intention is to make sure that only 
        /// one dll (this one) is needed as a dependency.
        /// </remarks>
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        /// <summary>
        /// Serializes the message as an info message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal override string SerializeAsInfo(string message)
        {
            return serializer.Serialize(new { info = message });
        }

        /// <summary>
        /// Serializes the message as a performance message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal override string SerializeAsPerformance(ServiceAppContext context, string message)
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
                    message = message
                }
            });
        }

        /// <summary>
        /// Serializes the message as a debug message.
        /// </summary>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal override string SerializeAsDebug(string message)
        {
            return serializer.Serialize(new { debug = message });
        }

        /// <summary>
        /// Serializes the message as an error message.
        /// </summary>
        /// <param name="exception">The exception to serialize</param>
        /// <param name="message">The message to serialize.</param>
        /// <returns></returns>
        internal override string SerializeAsError(Exception exception, string message)
        {
            return serializer.Serialize(new
            {
                error = new
                {
                    exception = exception.Message,
                    stackTrace = exception.StackTrace,
                    innerException = exception.InnerException != null ? exception.InnerException.Message : null,
                    innerStackTrace = exception.InnerException != null ? exception.InnerException.StackTrace : null,
                    message = message
                }
            });
        }

        /// <summary>
        /// Serializes the enum as a state message.
        /// </summary>
        /// <param name="state">The state.</param>
        internal override string SerializeAsState(Enums.State state)
        {
            return serializer.Serialize(new { state = Enum.GetName(typeof(Enums.State), state) });
        }
    }
}
