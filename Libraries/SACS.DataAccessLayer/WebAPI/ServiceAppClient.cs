using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The concrete service app web api client
    /// </summary>
    public class ServiceAppClient : WebApiClient, IServiceAppClient
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="ServiceAppClient "/> class
        /// </summary>
        /// <param name="baseAdderss"></param>
        /// <param name="httpMessageHandler"></param>
        internal ServiceAppClient(string baseAdderss, HttpMessageHandler httpMessageHandler)
            : base(baseAdderss, httpMessageHandler)
        {
        }

        /// <summary>
        /// Gets the list of installed service apps
        /// </summary>
        /// <returns></returns>
        public IList<ServiceApp> GetInstalledServiceApps()
        {
            return this.Get<IList<ServiceApp>>("ServiceApp");
        }

        /// <summary>
        /// Sends the command to start the specified service app by name
        /// </summary>
        /// <param name="appName"></param>
        public void StartServiceApp(string appName)
        {
            this.Get<string>(string.Format("ServiceApp/{0}/Start", appName));
        }

        /// <summary>
        /// Sends the command to stop the specified service app by name
        /// </summary>
        /// <param name="appName"></param>
        public void StopServiceApp(string appName)
        {
            this.Get<string>(string.Format("ServiceApp/{0}/Stop", appName));
        }

        /// <summary>
        /// Sends the command to run the specified service app by name
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void RunServiceApp(string appName)
        {
            this.Get<string>(string.Format("ServiceApp/{0}/Run", appName));
        }

        /// <summary>
        /// Updates the container with the specified service app
        /// </summary>
        /// <param name="serviceApp"></param>
        public void UpdateServiceApp(ServiceApp serviceApp)
        {
            this.Put<ServiceApp>(string.Format("ServiceApp/{0}/Update", serviceApp.Name), serviceApp);
        }

        /// <summary>
        /// Sends the command to remove the specified service app by name
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void RemoveServiceApp(string appName)
        {
            this.Delete<string>(string.Format("ServiceApp/{0}/Remove", appName));
        }

        public ServiceApp GetServiceApp(string appName)
        {
            throw new NotImplementedException();
        }
    }
}
