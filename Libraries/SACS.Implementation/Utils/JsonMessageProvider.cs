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
        // do the right-hand side first
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
    }
}
