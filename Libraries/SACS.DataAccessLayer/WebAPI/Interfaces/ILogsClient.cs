using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.WebAPI.Interfaces
{
    /// <summary>
    /// Web API client used for accessing Logs services
    /// </summary>
    public interface ILogsClient : IWebApiClient
    {
        /// <summary>
        /// Gets the current log names.
        /// </summary>
        /// <returns></returns>
        IList<string> GetLogNames();

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        /// <param name="logFileName">Name of the log file to get the entries from.</param>
        /// <returns></returns>
        IList<Models.LogEntry> GetLogEntries(string logFileName);
    }
}
