using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.WebAPI.Interfaces
{
    /// <summary>
    /// Web API client for retrieving server information
    /// </summary>
    public interface IServerClient : IWebApiClient
    {
        /// <summary>
        /// Gets the version information.
        /// </summary>
        /// <returns></returns>
        Version GetVersionInfo();
    }
}
