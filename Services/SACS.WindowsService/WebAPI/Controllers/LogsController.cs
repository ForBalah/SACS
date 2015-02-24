using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Logs;
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
        /// Gets the entries.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public IList<LogEntry> GetEntries(string id)
        {
            // id maps to the file name
            string fullPath = GetFullLogPath(id);
            IList<LogEntry> logs = new List<LogEntry>();

            try
            {
                LoadLogs(logs, fullPath);
            }
            catch (FileNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return logs;
        }

        /// <summary>
        /// Loads the logs.
        /// </summary>
        /// <param name="logs">The log list to load into.</param>
        /// <param name="fileName">Name of the file.</param>
        private static void LoadLogs(IList<LogEntry> logs, string fileName)
        {
            while (true)
            {
                try
                {
                    string xmlData = File.ReadAllText(fileName);
                    logLoader.LoadLogsFromXml(logs, LogLoader.AppendRoot(xmlData));
                    break;
                }
                catch (IOException)
                {
                    // If there is a lock on the file due to a current log write, retry after a few moments
                    Thread.Sleep(500);
                }
            }
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
            // TODO: might be able to get this from the Logger, instead of going via the config
            var log4netSection = XElement.Parse(((System.Xml.XmlElement)ConfigurationManager.GetSection("log4net")).OuterXml);

            var rollingLogAppender = log4netSection.Descendants("appender").FirstOrDefault(e => e.Attribute("type").Value == "log4net.Appender.RollingFileAppender");
            if (rollingLogAppender == null)
            {
                throw new ConfigurationErrorsException("log4net.Appender.RollingFileAppender not specified");
            }

            string fileName = (from param in rollingLogAppender.Descendants("param")
                                  where param.Attribute("name").Value == "File"
                                  select param.Attribute("value").Value).First();

            string fullFileName = Path.GetFullPath(fileName);

            var logPathNames = Directory.GetFiles(Path.GetDirectoryName(fullFileName), Path.GetFileName(fullFileName) + "*");

            return logPathNames.ToList();
        }
    }
}
