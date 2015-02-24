using System;
using System.Collections.Generic;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.DataAccess.Interfaces
{
    /// <summary>
    /// App List DAO interface
    /// </summary>
    public interface IAppListDao : IDao, IDisposable
    {
        /// <summary>
        /// Finds all the service apps
        /// </summary>
        /// <returns></returns>
        IEnumerable<ServiceApp> FindAll();

        /// <summary>
        /// Finds all the service apps, filtering by expression
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<ServiceApp> FindAll(Func<ServiceApp, bool> filter);

        /// <summary>
        /// Persists the service app.
        /// </summary>
        /// <param name="app">The application.</param>
        void PersistServiceApp(ServiceApp app);

        /// <summary>
        /// Deletes the service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void DeleteServiceApp(string appName);
    }
}
