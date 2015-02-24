using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The concrete logs web api client
    /// </summary>
    public class LogsClient : WebApiClient, ILogsClient
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="ServiceAppClient" /> class
        /// </summary>
        /// <param name="baseAdderss"></param>
        /// <param name="httpMessageHandler"></param>
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
        /// <returns></returns>
        public IList<Models.LogEntry> GetLogEntries(string logFileName)
        {
            return this.Get<IList<Models.LogEntry>>(string.Format("logs/{0}", logFileName));
        }
    }
}
