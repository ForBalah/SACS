using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The concrete server Web API client
    /// </summary>
    public class ServerClient : WebApiClient, IServerClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.DataAccessLayer.WebAPI.ServerClient" /> class
        /// </summary>
        /// <param name="baseAdderss">The base Web API url.</param>
        /// <param name="httpMessageHandler">The message handler class dependency.</param>
        internal ServerClient(string baseAdderss, HttpMessageHandler httpMessageHandler)
            : base(baseAdderss, httpMessageHandler)
        {
        }

        /// <summary>
        /// Gets the version information.
        /// </summary>
        /// <returns></returns>
        public Version GetVersionInfo()
        {
            return this.Get<Version>("Server/VersionInfo");
        }
    }
}
