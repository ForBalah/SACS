using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Implementation.Enums;
using SACS.Implementation.Execution;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Utility class that handles messages from the service app.
    /// </summary>
    public static class Messages
    {
        private static MessageProvider _Provider;

        /// <summary>
        /// Gets the message provider.
        /// </summary>
        public static MessageProvider Provider
        {
            get
            {
                if (_Provider == null)
                {
                    // default local dependency
                    _Provider = new JsonMessageProvider();
                }

                return _Provider;
            }

            internal set
            {
                _Provider = value;
            }
        }

        /// <summary>
        /// Writes the provided string as a "debug" message to the standard output stream.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WriteDebug(string format, params object[] args)
        {
            string cleanString = format ?? string.Empty;
            Console.WriteLine(Provider.SerializeAsDebug(string.Format(cleanString, args)));
        }

        /// <summary>
        /// Writes the provided string as an "info" message to the standard output stream.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WriteInfo(string format, params object[] args)
        {
            string cleanString = format ?? string.Empty;
            Console.WriteLine(Provider.SerializeAsInfo(string.Format(cleanString, args)));
        }

        /// <summary>
        /// Writes the provided string as an "error" message to the standard output stream.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void WriteError(Exception exception)
        {
            Console.WriteLine(Provider.SerializeAsError(exception));
        }

        /// <summary>
        /// Writes the provided string as a "performance" message to the standard output stream.
        /// </summary>
        /// <param name="context">The service app context to get the prformance information from.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        internal static void WritePerformance(ServiceAppContext context, string format, params object[] args)
        {
            string cleanString = format ?? string.Empty;
            Console.WriteLine(Provider.SerializeAsPerformance(context, string.Format(cleanString, args)));
        }

        /// <summary>
        /// Writes the provided string as a "performance" message to the standard output stream.
        /// </summary>
        /// <param name="state">The state to write.</param>
        internal static void WriteState(State state)
        {
            Console.WriteLine(Provider.SerializeAsState(state));
        }

        /// <summary>
        /// Writes the provided string as a "result" message to the standard output stream.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WriteResult(string format, params object[] args)
        {
            string cleanString = format ?? string.Empty;
            Console.WriteLine(Provider.SerializeAsResult(string.Format(cleanString, args)));
        }
    }
}