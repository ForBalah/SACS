using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.DTOs;

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
        /// <param name="page">The page number.</param>
        /// <param name="search">The search query.</param>
        /// <param name="size">The page size.</param>
        /// <returns></returns>
        PagingResult<Models.LogEntry> GetLogEntries(string logFileName, int? page, string search, int? size = null);
    }
}
