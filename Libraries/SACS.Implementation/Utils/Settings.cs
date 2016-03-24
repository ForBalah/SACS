using System.Configuration;
using System.Reflection;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Acts as an AppSettings helper class.
    /// </summary>
    public class Settings
    {
        private static bool? logToFile;
        private static string customParameters;

        /// <summary>
        /// Gets or sets a value indicating whether activity in the service app should be dumped to a
        /// file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If set to <c>true</c> key service app activity will be dumped in a file, called dump.txt.
        /// This value can also be sourced from AppSettings as SACS:DumpToFile. The default is <c>false</c>.
        /// </para>
        /// <para>
        /// Set this property either in the app.config or before the service app is started to catch all app
        /// activity. The file will contain all activity that failed to be sent back to the SAC for logging.
        /// </para>
        /// </remarks>
        public static bool LogToFile
        {
            get
            {
                if (!logToFile.HasValue)
                {
                    string setting = ConfigurationManager.AppSettings["SACS:LogToFile"];
                    bool value = false;

                    if (bool.TryParse(setting ?? "false", out value))
                    {
                        logToFile = value;
                    }
                    else
                    {
                        throw new ConfigurationErrorsException("SACS:DumpToFile is not in the correct format. Must be \"true\" or \"false\"");
                    }
                }

                return logToFile ?? false;
            }

            set
            {
                logToFile = value;
            }
        }

        /// <summary>
        /// Gets the maximum allowed concurrent exections.
        /// </summary>
        /// <remarks>
        /// This is sourced from AppSettings as SACS:MaxConcurrentExecutions. The default is 5.
        /// </remarks>
        public static int MaxConcurrentExecutions
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["SACS:MaxConcurrentExecutions"];
                int value = 5;

                if (int.TryParse(setting, out value) && value >= 0)
                {
                    return value;
                }
                else
                {
                    throw new ConfigurationErrorsException("SACS:MaxConcurrentExecutions is not in the correct format or is less than zero.");
                }
            }
        }

        /// <summary>
        /// Gets the alternate name to use for this service app if it has not been supplied by the container.
        /// </summary>
        /// <remarks>
        /// This is sourced from AppSettings as SACS:AlternateName. The default is the assembly it is in.
        /// </remarks>
        public static string AlternateName
        {
            get
            {
                string setting = ConfigurationManager.AppSettings["SACS:AlternateName"];
                return setting ?? (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).FullName;
            }
        }

        /// <summary>
        /// Gets the custom parameters passed into the service app.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Parameters can be any string and can contain any information that the service app implementation needs.
        /// For example a list of key=value pairs can go in here, and they can be interpreted and used to modify
        /// behaviour of different service app instances.
        /// </para>
        /// <para>
        /// These parameters can especially come in handy when the same service app is loaded into SACS multiple
        /// times - typically for different schedules - and each service app needs to perform slightly different
        /// tasks on the same data
        /// </para>
        /// <para>
        /// This value is set either from AppSettings as SACS:Parameters, or can be passed in as a command parameter:
        /// { parameters: "[data]" }
        /// </para>
        /// </remarks>
        public static string Parameters
        {
            get
            {
                return customParameters ?? ConfigurationManager.AppSettings["SACS:Parameters"];
            }
        }

        /// <summary>
        /// Sets the parameters value
        /// </summary>
        /// <param name="value">The value to set.</param>
        internal static void SetParameters(string value)
        {
            customParameters = value;
        }
    }
}