using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.DataAccess.Interfaces
{
    /// <summary>
    /// DAO for SACS system processes
    /// </summary>
    public interface ISystemDao : IGenericDao
    {
        /// <summary>
        /// Logs the system performances.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cpuValue">The cpu value.</param>
        /// <param name="ramValue">The ram value.</param>
        void LogSystemPerformances(string message, decimal? cpuValue, decimal? ramValue);

        /// <summary>
        /// Gets the cpu performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IList<Models.SystemPerformance> GetCpuPerformanceData(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the memory performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IList<Models.SystemPerformance> GetMemoryPerformanceData(DateTime fromDate, DateTime toDate);
    }
}
