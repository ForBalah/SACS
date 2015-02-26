using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;

namespace SACS.WindowsService.WebAPI.Controllers
{
    /// <summary>
    /// The AppPerformance web api controller
    /// </summary>
    public class PerformanceController : ApiController
    {
        private readonly IAppPerformanceDao _perfDao = DaoFactory.Create<IAppPerformanceDao, AppPerformanceDao>();
        private readonly ISystemDao _systemDao = DaoFactory.Create<ISystemDao, SystemDao>();

        /// <summary>
        /// Gets the application performance data.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns></returns>
        /// <remarks>
        /// Dates must be formatted as YYYYMMDD.
        /// </remarks>
        [HttpGet]
        public IDictionary<string, IList<AppPerformance>> GetAppPerformanceData(string from, string to)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(from, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, "yyyyMMdd", CultureInfo.InvariantCulture);

                return this._perfDao.GetAppPerformanceData(fromDate, toDate);
            }
            catch (FormatException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets the system cpu performance data.
        /// </summary>
        /// <param name="cpuFrom">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Cpu")]
        public IList<SystemPerformance> GetSystemCpuPerformanceData(string cpuFrom, string to)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(cpuFrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, "yyyyMMdd", CultureInfo.InvariantCulture);

                return this._systemDao.GetCpuPerformanceData(fromDate, toDate);
            }
            catch (FormatException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets the system memory performance data.
        /// </summary>
        /// <param name="memFrom">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Memory")]
        public IList<SystemPerformance> GetSystemMemoryPerformanceData(string memFrom, string to)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(memFrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, "yyyyMMdd", CultureInfo.InvariantCulture);

                return this._systemDao.GetMemoryPerformanceData(fromDate, toDate);
            }
            catch (FormatException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
