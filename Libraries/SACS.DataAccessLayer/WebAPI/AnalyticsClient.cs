﻿using System;
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
        /// The date format
        /// </summary>
        private const string DateFormat = "yyyyMMddHHmm";

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
                { "from", fromDate.ToString(DateFormat) },
                { "to", toDate.ToString(DateFormat) },
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
                { "cpuFrom", fromDate.ToString(DateFormat) },
                { "to", toDate.ToString(DateFormat) },
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
                { "memFrom", fromDate.ToString(DateFormat) },
                { "to", toDate.ToString(DateFormat) },
            };

            return this.Get<IList<SystemPerformance>>("Performance/Memory", parameters);
        }

        /// <summary>
        /// Sends a performance refresh request. Usually involves adding new values to the database.
        /// </summary>
        /// <returns></returns>
        public bool RequestPerformanceRefresh()
        {
            this.Post<string>("Performance/Refresh", null);
            return true;
        }
    }
}