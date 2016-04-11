using System;
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
            FileLogger.Log("Sent debug data: " + cleanString);
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
            FileLogger.Log("Sent information: " + cleanString);
        }

        /// <summary>
        /// Writes the provided string as an "error" message to the standard output stream.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void WriteError(Exception exception)
        {
            Console.WriteLine(Provider.SerializeAsError(exception));
            FileLogger.Log("Sent exception");
        }

        /// <summary>
        /// Writes the provided string as a "performance" message to the standard output stream.
        /// </summary>
        /// <param name="context">The service app context to get the prformance information from.</param>
        internal static void WritePerformance(ServiceAppContext context)
        {
            Console.WriteLine(Provider.SerializeAsPerformance(context));
            FileLogger.Log("Sent performance information");
        }

        /// <summary>
        /// Writes the provided string as a "performance" message to the standard output stream.
        /// </summary>
        /// <param name="state">The state to write.</param>
        internal static void WriteState(State state)
        {
            Console.WriteLine(Provider.SerializeAsState(state));
            FileLogger.Log("Sent state: " + Enum.GetName(typeof(State), state));
        }

        /// <summary>
        /// Writes the proviced version as a string to the standard output stream.
        /// </summary>
        /// <param name="version">The version to write to the stream.</param>
        internal static void WriteVersion(Version version)
        {
            Console.WriteLine(Provider.SerializeAsVersion(version));
            FileLogger.Log("Sent version: " + version.ToString());
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
            FileLogger.Log("Sent result: " + cleanString);
        }
    }
}