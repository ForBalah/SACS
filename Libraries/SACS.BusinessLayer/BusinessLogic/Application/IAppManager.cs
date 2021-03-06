﻿using System.Collections.Generic;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Application
{
    /// <summary>
    /// Manages the service apps (as ServiceAppProcesses) from start to stop including
    /// coordinating the database saving/loading of them.
    /// </summary>
    public interface IAppManager
    {
        #region Properties

        /// <summary>
        /// Gets all service apps.
        /// </summary>
        /// <returns></returns>
        IList<ServiceApp> ServiceApps { get; }

        /// <summary>
        /// Gets the service application domains.
        /// </summary>
        /// <value>
        /// The service application domains.
        /// </value>
        ServiceAppProcessCollection ServiceAppProcesses { get; }

        #endregion Properties

        #region Methods

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
        /// <returns>A string containing an error message if a recoverable error occured.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The ServiceApp could not be found in the container.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp is not in a stopped state.</exception>
        string InitializeServiceApp(string appName, IServiceAppDao dao);

        /// <summary>
        /// Stops all running service apps.
        /// </summary>
        /// <param name="dao">The ServiceApp DAO</param>
        /// <param name="isExiting">Set to <c>true</c> if the service is in the middle of exiting.</param>
        void StopAllServiceApps(IServiceAppDao dao, bool isExiting);

        /// <summary>
        /// Stops the specified service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="isExiting">Set to <c>true</c> if the service is in the middle of exiting.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        void StopServiceApp(string appName, IServiceAppDao dao, bool isExiting);

        /// <summary>
        /// Adds the service app to the container or updates it if it already exists and saves it to the AppList configuration.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <exception cref="System.InvalidOperationException">The service app is added and is still running.</exception>
        string SyncServiceApp(ServiceApp app, IServiceAppDao dao, IAppListDao appListDao);

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
        void RunServiceApp(string appName);

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

        #endregion Methods
    }
}