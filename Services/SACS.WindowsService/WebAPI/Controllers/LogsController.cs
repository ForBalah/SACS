using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using log4net;
using log4net.Appender;
using SACS.BusinessLayer.BusinessLogic.Logs;
using SACS.Common.DTOs;
using SACS.DataAccessLayer.Models;

namespace SACS.WindowsService.WebAPI.Controllers
{
    /// <summary>
    /// The Logs api controller
    /// </summary>
    public class LogsController : ApiController
    {
        private static ILog log = LogManager.GetLogger(typeof(LogsController));
        private static LogLoader logLoader = new LogLoader(LogLoader.DefaultDate);

        /// <summary>
        /// Gets the log names.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetLogNames()
        {
            try
            {
                return GetNamesFromRollingLogConfiguration();
            }
            catch (ConfigurationErrorsException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the entries. GET /logs/[logname]?page=&amp;search=
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="page">The page.</param>
        /// <param name="search">The search.</param>
        /// <param name="size">The paging sizesize.</param>
        /// <returns></returns>
        /// <exception cref="System.Web.Http.HttpResponseException">Thrown when the log is not found.</exception>
        public PagingResult<LogEntry> GetEntries(string id, int? page = null, string search = null, int size = 0)
        {
            log.Debug(string.Format("(API) Log -> GetEntries - {0}", id));

            // id maps to the file name
            string fullPath = GetFullLogPath(id);
            IList<LogEntry> logs = new List<LogEntry>();

            try
            {
                logLoader.LoadLogs(logs, fullPath);
            }
            catch (FileNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return LogLoader.FilterLogs(logs, page, search, size);
        }

        /// <summary>
        /// Gets the full log path.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private static string GetFullLogPath(string file)
        {
            return GetNamesFromRollingLogConfiguration().FirstOrDefault(o => o.EndsWith(file));
        }

        /// <summary>
        /// Gets the names from log configuration - specifically the rolling file log.
        /// </summary>
        /// <returns></returns>
        private static IList<string> GetNamesFromRollingLogConfiguration()
        {
            var rootAppender = LogManager.GetRepository()
                                  .GetAppenders()
                                  .OfType<FileAppender>()
                                  .FirstOrDefault();

            var logPathNames = Directory.GetFiles(Path.GetDirectoryName(rootAppender.File), Path.GetFileName(rootAppender.File) + "*");
            return logPathNames.ToList();
        }
    }
}