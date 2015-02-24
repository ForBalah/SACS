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
        /// Gets the encryption key.
        /// </summary>
        /// <value>
        /// The encryption key.
        /// </value>
        string EncryptionSecretKey { get; }
    }
}
