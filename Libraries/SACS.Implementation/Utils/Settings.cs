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
    /// Acts as an AppSettings helper class
    /// </summary>
    public class Settings
    {
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
