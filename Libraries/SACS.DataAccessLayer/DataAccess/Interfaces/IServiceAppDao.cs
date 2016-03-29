using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.DataAccess.Interfaces
{
    /// <summary>
    /// ServiceAppDao interface
    /// </summary>
    public interface IServiceAppDao : IGenericDao
    {
        /// <summary>
        /// Saves the service app, updating the existing record, if found, or creating a new record.
        /// </summary>
        /// <param name="app">The service app.</param>
        /// <returns>The entropy value for the service app.</returns>
        string SaveServiceApp(Models.ServiceApp app);

        /// <summary>
        /// Records the service application start.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void RecordServiceAppStart(string appName);

        /// <summary>
        /// Records the service application stop.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        void RecordServiceAppStop(string appName);

        /// <summary>
        /// Records the perfromance for the service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="performance">The performance.</param>
        void RecordPerfromance(string appName, AppPerformance performance);

        /// <summary>
        /// Gets the service applications active history.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, bool> GetServiceAppActiveHistory();

        /// <summary>
        /// Deletes the service app.
        /// </summary>
        /// <param name="appName">Name of the app.</param>
        void DeleteServiceApp(string appName);

        /// <summary>
        /// Gets the last start and end date for a service app
        /// </summary>
        /// <param name="appName">The app name to get the perf details for.</param>
        /// <returns></returns>
        Tuple<DateTime, DateTime?> GetLastRunDate(string appName);
    }
}