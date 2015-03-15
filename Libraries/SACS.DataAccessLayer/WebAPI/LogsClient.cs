using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;
using SACS.Common.DTOs;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The concrete logs web api client
    /// </summary>
    public class LogsClient : WebApiClient, ILogsClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.DataAccessLayer.WebAPI.LogsClient" /> class.
        /// </summary>
        /// <param name="baseAdderss">The base Web API url.</param>
        /// <param name="httpMessageHandler">The message handler class dependency.</param>
        internal LogsClient(string baseAdderss, HttpMessageHandler httpMessageHandler)
            : base(baseAdderss, httpMessageHandler)
        {
        }

        /// <summary>
        /// Gets the current log names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetLogNames()
        {
            return this.Get<IList<string>>("Logs");
        }

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        /// <param name="logFileName">Name of the log file to get the entries from.</param>
        /// <param name="page">The page number.</param>
        /// <param name="search">The search query.</param>
        /// <param name="size">The page size.</param>
        /// <returns></returns>
        public PagingResult<Models.LogEntry> GetLogEntries(string logFileName, int? page, string search, int? size = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (page.HasValue)
            {
                parameters.Add("page", page.ToString());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                parameters.Add("search", search);
            }

            if (size == null)
            {
                parameters.Add("size", ApplicationSettings.Current.DefaultPagingSize.ToString());
            }
            else
            {
                parameters.Add("size", size.ToString());
            }

            return this.Get<PagingResult<Models.LogEntry>>(string.Format("logs/{0}", logFileName), parameters);
        }
    }
}
