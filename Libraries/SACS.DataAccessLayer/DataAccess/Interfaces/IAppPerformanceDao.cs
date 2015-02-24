using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.DataAccess.Interfaces
{
    /// <summary>
    /// DAO designed to work with App Performance data
    /// </summary>
    public interface IAppPerformanceDao : IGenericDao
    {
        /// <summary>
        /// Gets the application performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IDictionary<string, IList<Models.AppPerformance>> GetAppPerformanceData(DateTime fromDate, DateTime toDate);
    }
}
