using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.WebAPI.Interfaces
{
    /// <summary>
    /// Web API client used for accessing log and analytics services
    /// </summary>
    public interface IAnalyticsClient : IWebApiClient
    {
        /// <summary>
        /// Gets all the application performances given the specified date range
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IDictionary<string, IList<AppPerformance>> GetAllAppPerformances(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the system cpu performance.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IList<SystemPerformance> GetSystemCpuPerformance(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the system memory performance.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IList<SystemPerformance> GetSystemMemoryPerformance(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Sends a performance refresh request. Usually involves adding new values to the database.
        /// </summary>
        /// <returns></returns>
        bool RequestPerformanceRefresh();
    }
}