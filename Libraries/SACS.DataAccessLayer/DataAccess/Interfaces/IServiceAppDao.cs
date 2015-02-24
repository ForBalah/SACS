using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void SaveServiceApp(Models.ServiceApp app);

        /// <summary>
        /// Records the service app start.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <returns>
        /// An integer reference to the performance record.
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">ServiceApplication to log against could not be found</exception>
        int RecordServiceAppExecutionStart(string appName);

        /// <summary>
        /// Records the service app execution end.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="performanceId">The performance identifier.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">ServiceApplication to log against could not be found</exception>
        void RecordServiceAppExecutionEnd(string appName, int performanceId);

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
    }
}
