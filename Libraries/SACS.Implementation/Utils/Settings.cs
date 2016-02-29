using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Acts as an AppSettings helper class.
    /// </summary>
    public class Settings
    {
        private static bool? _DumpToFile;

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
        public static bool DumpToFile
        {
            get
            {
                if (!_DumpToFile.HasValue)
                {
                    string setting = ConfigurationManager.AppSettings["SACS:DumpToFile"];
                    bool value = false;

                    if (bool.TryParse(setting ?? "false", out value))
                    {
                        _DumpToFile = value;
                    }
                    else
                    {
                        throw new ConfigurationErrorsException("SACS:DumpToFile is not in the correct format. Must be \"true\" or \"false\"");
                    }
                }

                return _DumpToFile ?? false;
            }

            set
            {
                _DumpToFile = value;
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
    }
}