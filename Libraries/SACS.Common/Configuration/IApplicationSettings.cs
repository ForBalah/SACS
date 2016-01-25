using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Configuration
{
    /// <summary>
    /// The Application configuration interface
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// Gets the application list location.
        /// </summary>
        /// <value>
        /// The application list location.
        /// </value>
        string AppListLocation { get; }

        /// <summary>
        /// Gets the default paging size.
        /// </summary>
        /// <value>
        /// The default paging size.
        /// </value>
        int DefaultPagingSize { get; }

        /// <summary>
        /// Gets the encryption key.
        /// </summary>
        /// <value>
        /// The encryption key.
        /// </value>
        byte[] EntropyValue { get; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        string ServiceName { get; }

        /// <summary>
        /// Gets the support email template path.
        /// </summary>
        /// <value>
        /// The support email template path.
        /// </value>
        string SupportEmailTemplatePath { get; }

        /// <summary>
        /// Gets the successful execution email template path.
        /// </summary>
        /// <value>
        /// The successful exeuction email template path.
        /// </value>
        string SuccessEmailTemplatePath { get; }

        /// <summary>
        /// Gets the support email address.
        /// </summary>
        /// <value>
        /// The support email address.
        /// </value>
        string SupportEmailAddress { get; }

        /// <summary>
        /// Gets the Web API base address
        /// </summary>
        string WebApiBaseAddress { get; }

        /// <summary>
        /// Gets the Max points setting for graphs in the performance section
        /// </summary>
        int PerformanceGraphMaxPoints { get; }

        /// <summary>
        /// Gets the threshold setting for graphs in the performance section
        /// </summary>
        decimal PerformanceGraphThreshold { get; }

        /// <summary>
        /// Gets the alternate log location.
        /// </summary>
        /// <value>
        /// The alternate log location.
        /// </value>
        string AlternateLogLocation { get; }

        /// <summary>
        /// Gets the timeout (in seconds) to use for Web API requests
        /// </summary>
        int WebApiTimeout { get; }
    }
}