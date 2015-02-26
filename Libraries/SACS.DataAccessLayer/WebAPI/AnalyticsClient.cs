using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The concrete analytics web api client
    /// </summary>
    public class AnalyticsClient : WebApiClient, IAnalyticsClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.DataAccessLayer.WebAPI.AnalyticsClient" /> class
        /// </summary>
        /// <param name="baseAdderss">The base Web API url.</param>
        /// <param name="httpMessageHandler">The message handler class dependency.</param>
        internal AnalyticsClient(string baseAdderss, HttpMessageHandler httpMessageHandler)
            : base(baseAdderss, httpMessageHandler)
        {
        }

        /// <summary>
        /// Gets all the application performances given the specified date range
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IDictionary<string, IList<AppPerformance>> GetAllAppPerformances(DateTime fromDate, DateTime toDate)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "from", fromDate.ToString("yyyyMMdd") },
                { "to", toDate.ToString("yyyyMMdd") },
            };

            return this.Get<IDictionary<string, IList<AppPerformance>>>("Performance", parameters);
        }

        /// <summary>
        /// Gets the system cpu performance.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IList<SystemPerformance> GetSystemCpuPerformance(DateTime fromDate, DateTime toDate)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "cpuFrom", fromDate.ToString("yyyyMMdd") },
                { "to", toDate.ToString("yyyyMMdd") },
            };

            return this.Get<IList<SystemPerformance>>("Performance/Cpu", parameters);
        }

        /// <summary>
        /// Gets the system memory performance.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IList<SystemPerformance> GetSystemMemoryPerformance(DateTime fromDate, DateTime toDate)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "memFrom", fromDate.ToString("yyyyMMdd") },
                { "to", toDate.ToString("yyyyMMdd") },
            };

            return this.Get<IList<SystemPerformance>>("Performance/Memory", parameters);
        }
    }
}
