using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SACS.Common.Configuration;
using SACS.WindowsService.Components;

namespace SACS.WindowsService.WebAPI.Controllers
{
    /// <summary>
    /// The server Wep API controller
    /// </summary>
    public class ServerController : ApiController
    {
        /// <summary>
        /// Gets the version the information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Server/VersionInfo")]
        public Version VersionInfo()
        {
            return typeof(ServiceContainer).Assembly.GetName().Version;
        }

        /// <summary>
        /// Gets the support email address used by the system.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Server/SupportEmail")]
        public string GetSupportEmailAddress()
        {
            return ApplicationSettings.Current.SupportEmailAddress;
        }
    }
}
