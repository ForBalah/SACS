using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.WebAPI.Interfaces
{
    /// <summary>
    /// Web API client used for accessing Service App services
    /// </summary>
    public interface IServiceAppClient : IWebApiClient
    {
        /// <summary>
        /// Gets the list of installed service apps
        /// </summary>
        /// <returns></returns>
        IList<ServiceApp> GetInstalledServiceApps();

        /// <summary>
        /// Sends the command to start the specified service app by name
        /// </summary>
        /// <param name="appName"></param>
        void StartServiceApp(string appName);

        /// <summary>
        /// Sends the command to stop the specified service app by name
        /// </summary>
        /// <param name="appName"></param>
        void StopServiceApp(string appName);

        /// <summary>
        /// Sends the command to run the specified service app by name
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void RunServiceApp(string appName);

        /// <summary>
        /// Updates the container with the specified service app
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void UpdateServiceApp(ServiceApp serviceApp);

        /// <summary>
        /// Sends the command to remove the specified service app by name
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void RemoveServiceApp(string appName);

        /// <summary>
        /// Gets the specified service app by name
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        ServiceApp GetServiceApp(string appName);
    }
}
