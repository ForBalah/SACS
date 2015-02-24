using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Application
{
    /// <summary>
    /// Interface for the AppManager methods
    /// </summary>
    public interface IAppManager
    {
        #region Properties

        /// <summary>
        /// Gets or sets the scheduling service.
        /// </summary>
        /// <value>
        /// The scheduling service.
        /// </value>
        IServiceAppSchedulingService SchedulingService { get; set; }

        /// <summary>
        /// Gets all service apps.
        /// </summary>
        /// <returns></returns>
        IList<ServiceApp> ServiceApps { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the list of app domains that have been loaded.
        /// </summary>
        /// <returns></returns>
        IList<AppDomain> GetDomains();

        /// <summary>
        /// Loads all the service apps and schedules them.
        /// </summary>
        /// <param name="appList">The application list.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="errorList">The error list.</param>
        void InitializeAllServiceApps(IEnumerable<ServiceApp> appList, IServiceAppDao dao, IList<string> errorList);

        /// <summary>
        /// Loads the service app which should be added to the pool of service apps already.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="startAutomatically">If set to <c>true</c> schedule automatically.</param>
        /// <returns>A string containing an error message if a recoverable error occured.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The ServiceApp could not be found in the container.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp is not in a stopped state.</exception>
        string InitializeServiceApp(string appName, IServiceAppDao dao);

        /// <summary>
        /// Stops all running service apps.
        /// </summary>
        /// <param name="dao">The ServiceApp DAO</param>
        void StopAllServiceApps(IServiceAppDao dao);

        /// <summary>
        /// Stops the specified service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        void StopServiceApp(string appName, IServiceAppDao dao);

        /// <summary>
        /// Adds the service app to the container or updates it if it already exists and saves it to the AppList configuration.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <exception cref="System.InvalidOperationException">The service app is added and is still running.</exception>
        string PersistServiceApp(ServiceApp app, IServiceAppDao dao, IAppListDao appListDao);

        /// <summary>
        /// Removes the specified service app from the container without removing from the app List.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp was not stopped first.</exception>
        void RemoveServiceApp(string appName, IServiceAppDao dao);

        /// <summary>
        /// Removes the specified service app from the container.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp was not stopped first.</exception>
        void RemoveServiceApp(string appName, IServiceAppDao dao, IAppListDao appListDao);

        /// <summary>
        /// Runs the service application immediately.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        void RunServiceApp(string appName, IServiceAppDao dao);

        /// <summary>
        /// Updates the service app in the container or adds a new one to it if it does not exist.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <returns>
        /// An error message if there was a problem updating the service app
        /// </returns>
        string UpdateServiceApp(ServiceApp serviceApp, IServiceAppDao dao, IAppListDao appListDao);

        #endregion
    }
}
