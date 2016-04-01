using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using log4net;
using SACS.Common.Configuration;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;
using SACS.WindowsService.Common;
using SACS.WindowsService.Components;

namespace SACS.WindowsService.WebAPI.Controllers
{
    /// <summary>
    /// The AppPerformance web api controller
    /// </summary>
    public class PerformanceController : ApiController
    {
        private const int SecondsInterval = 90;
        private static ILog log = LogManager.GetLogger(typeof(PerformanceController));
        private readonly IAppPerformanceDao _perfDao = DaoFactory.Create<IAppPerformanceDao, AppPerformanceDao>(); // TODO: delete
        private readonly IDaoFactory _daoFactory;
        private readonly SystemMonitor _systemMonitor;

        public PerformanceController(SystemMonitor systemMonitor, IDaoFactory daoFactory)
        {
            _systemMonitor = systemMonitor;
            _daoFactory = daoFactory;
        }

        /// <summary>
        /// Gets the application performance data.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <param name="apps">The apps.</param>
        /// <returns></returns>
        /// <remarks>
        /// Dates must be formatted as yyyyMMddHHmm (or as overridden in the <see cref="SACS.WindowsService.Common.Constants" />).
        /// </remarks>
        [HttpGet]
        [Obsolete("Dropped in favour of a 3rd party analytics app")]
        public IDictionary<string, IList<AppPerformance>> GetAverageAppPerformanceData(string from, string to, string apps = null)
        {
            List<string> appList = new List<string>();

            if (apps != null)
            {
                appList = apps.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            try
            {
                DateTime fromDate = DateTime.ParseExact(from, Constants.DateFormat, CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, Constants.DateFormat, CultureInfo.InvariantCulture);

                return this._perfDao.GetAppPerformanceData(fromDate, toDate, appList);
            }
            catch (Exception e)
            {
                log.Error("REST API Error", e);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets the system cpu performance data.
        /// </summary>
        /// <param name="cpuFrom">From date.</param>
        /// <param name="to">To date.</param>
        /// <param name="raw">Indicates whether to return raw data. Default is to return compacted data.</param>
        /// <returns></returns>
        /// <remarks>
        /// Dates must be formatted as yyyyMMddHHmm (or as overridden in the <see cref="SACS.WindowsService.Common.Constants" />).
        /// </remarks>
        [HttpGet]
        [ActionName("Cpu")]
        public IList<SystemPerformance> GetSystemCpuPerformanceData(string cpuFrom, string to, bool? raw = null)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(cpuFrom, Constants.DateFormat, CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, Constants.DateFormat, CultureInfo.InvariantCulture);

                using (var dao = _daoFactory.Create<ISystemDao>())
                {
                    var data = dao.GetCpuPerformanceData(fromDate, toDate);

                    if (raw != true)
                    {
                        SystemPerformance.CompactData(data, SecondsInterval);
                        SystemPerformance.LowerResolution(
                            data,
                            ApplicationSettings.Current.PerformanceGraphMaxPoints,
                            ApplicationSettings.Current.PerformanceGraphThreshold);
                    }

                    return data;
                }
            }
            catch (Exception e)
            {
                log.Error("REST API Error", e);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets the system memory performance data.
        /// </summary>
        /// <param name="memFrom">From date.</param>
        /// <param name="to">To date.</param>
        /// <param name="raw">Indicates whether to return raw data. Default is to return compacted data.</param>
        /// <returns></returns>
        /// <remarks>
        /// Dates must be formatted as yyyyMMddHHmm (or as overridden in the <see cref="SACS.WindowsService.Common.Constants" />).
        /// </remarks>
        [HttpGet]
        [ActionName("Memory")]
        public IList<SystemPerformance> GetSystemMemoryPerformanceData(string memFrom, string to, bool? raw = null)
        {
            try
            {
                DateTime fromDate = DateTime.ParseExact(memFrom, Constants.DateFormat, CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(to, Constants.DateFormat, CultureInfo.InvariantCulture);

                using (var dao = _daoFactory.Create<ISystemDao>())
                {
                    var data = dao.GetMemoryPerformanceData(fromDate, toDate);

                    if (raw != true)
                    {
                        SystemPerformance.CompactData(data, SecondsInterval);
                        SystemPerformance.LowerResolution(
                            data,
                            ApplicationSettings.Current.PerformanceGraphMaxPoints,
                            ApplicationSettings.Current.PerformanceGraphThreshold);
                    }

                    return data;
                }
            }
            catch (Exception e)
            {
                log.Error("REST API Error", e);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Posts a request to the server to refresh the CPU and memory data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Refresh")]
        public bool RequestPerformanceRefresh()
        {
            log.Debug("(API) Performance -> RequestPerformanceRefresh");
            _systemMonitor.RecordPerformance();
            return true;
        }
    }
}