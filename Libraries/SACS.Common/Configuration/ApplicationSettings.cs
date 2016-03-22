using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Configuration
{
    /// <summary>
    /// Main concrete implementation of Application Configuration
    /// </summary>
    public class ApplicationSettings : IApplicationSettings
    {
        #region Fields

        private static object _syncRoot = new object();
        private static IApplicationSettings _Instance;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ApplicationSettings"/> class from being created.
        /// </summary>
        private ApplicationSettings()
        {
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets the current instance of the application configuration
        /// </summary>
        public static IApplicationSettings Current
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_Instance == null)
                    {
                        _Instance = new ApplicationSettings();
                    }
                }

                return _Instance;
            }

            internal set
            {
                _Instance = value;
            }
        }

        /// <summary>
        /// Gets the application list location.
        /// </summary>
        /// <value>
        /// The application list location.
        /// </value>
        public string AppListLocation
        {
            get
            {
                return ConfigurationManager.AppSettings["AppListLocation"];
            }
        }

        /// <summary>
        /// Gets the default paging size.
        /// </summary>
        /// <value>
        /// The default paging size.
        /// </value>
        public int DefaultPagingSize
        {
            get
            {
                int pageSize;
                if (int.TryParse(ConfigurationManager.AppSettings["System.DefaultPagingSize"], out pageSize))
                {
                    return pageSize;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the entropy which makes it harder to decrypt secured values.
        /// </summary>
        /// <value>
        /// The entropy value.
        /// </value>
        public byte[] EntropyValue
        {
            get
            {
                return Encoding.Unicode.GetBytes(ConfigurationManager.AppSettings["Security.EntropyValue"] ?? string.Empty);
            }
        }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName
        {
            get
            {
                return ConfigurationManager.AppSettings["ServiceName"];
            }
        }

        /// <summary>
        /// Gets the support email template path.
        /// </summary>
        /// <value>
        /// The support email template path.
        /// </value>
        public string SupportEmailTemplatePath
        {
            get
            {
                return IfNullOrEmpty(ConfigurationManager.AppSettings["System.SupportEmailTemplate"], "Templates/Email/SupportEmailTemplate.html");
            }
        }

        /// <summary>
        /// Gets the successful execution email template path.
        /// </summary>
        /// <value>
        /// The successful exeuction email template path.
        /// </value>
        public string SuccessEmailTemplatePath
        {
            get
            {
                return IfNullOrEmpty(ConfigurationManager.AppSettings["System.SuccessEmailTemplate"], "Templates/Email/SuccessEmailTemplate.html");
            }
        }

        /// <summary>
        /// Gets the support email address.
        /// </summary>
        /// <value>
        /// The support email address.
        /// </value>
        public string SupportEmailAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["System.SupportEmailAddress"];
            }
        }

        /// <summary>
        /// Gets the Web API base address
        /// </summary>
        public string WebApiBaseAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["WebAPI.BaseAddress"];
            }
        }

        /// <summary>
        /// Gets the Max points setting for graphs in the performance section
        /// </summary>
        public int PerformanceGraphMaxPoints
        {
            get
            {
                return int.Parse(IfNullOrEmpty(ConfigurationManager.AppSettings["Performance.GraphMaxPoints"], "0"), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the threshold setting for graphs in the performance section
        /// </summary>
        public decimal PerformanceGraphThreshold
        {
            get
            {
                return decimal.Parse(IfNullOrEmpty(ConfigurationManager.AppSettings["Performance.GraphThreshold"], "0.0"), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the alternate log location. This applies mainly to the windows management console.
        /// </summary>
        /// <value>
        /// The alternate log location.
        /// </value>
        public string AlternateLogLocation
        {
            get
            {
                return ConfigurationManager.AppSettings["Logs.AlternateLocation"];
            }
        }

        /// <summary>
        /// Gets the timeout (in seconds) to use for Web API requests
        /// </summary>
        public int WebApiTimeout
        {
            get
            {
                return int.Parse(IfNullOrEmpty(ConfigurationManager.AppSettings["WebAPI.Timeout"], "15"), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets a value indicating whether separate user logins are enabled in this version.
        /// </summary>
        public bool EnableCustomUserLogins
        {
            get
            {
                return bool.Parse(IfNullOrEmpty(ConfigurationManager.AppSettings["Runtime.EnableCustomUserLogins"], "true"));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns the replacement string if the specified string is null or empty, otherwise it will return the string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string IfNullOrEmpty(string value, string replacement)
        {
            if (string.IsNullOrEmpty(value))
            {
                return replacement;
            }

            return value;
        }

        #endregion Methods
    }
}