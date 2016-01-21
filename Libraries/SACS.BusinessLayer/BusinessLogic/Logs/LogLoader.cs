using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using SACS.Common.DTOs;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Logs
{
    /// <summary>
    /// The class responsible for loading logs
    /// </summary>
    public class LogLoader
    {
        /// <summary>
        /// The default namespace
        /// </summary>
        public const string DefaultNamespace = "log4j";

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.BusinessLayer.BusinessLogic.Logs.LogLoader"/> class.
        /// </summary>
        /// <param name="baseDate">The base date to use to transform logged dates with.</param>
        public LogLoader(DateTime baseDate)
        {
            this.BaseDate = baseDate;
        }

        #region Properties

        /// <summary>
        /// Gets the default base date
        /// </summary>
        public static DateTime DefaultDate
        {
            get
            {
                return new DateTime(1970, 1, 1);
            }
        }

        /// <summary>
        /// Gets the base date.
        /// </summary>
        /// <value>
        /// The base date.
        /// </value>
        public DateTime BaseDate { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Filters the logs.
        /// </summary>
        /// <param name="logs">The logs to filter.</param>
        /// <param name="page">The page of results to return.</param>
        /// <param name="search">The search query (if any) to filter by.</param>
        /// <param name="size">The size of the pages.</param>
        /// <returns></returns>
        public static PagingResult<LogEntry> FilterLogs(IList<LogEntry> logs, int? page, string search, int size)
        {
            LogSearchCriteria searchCriteria = new LogSearchCriteria { PageNumber = page ?? 0, SearchQuery = search, PagingSize = size };
            var filteredLogs = searchCriteria.FilterLogs(logs);
            return new PagingResult<LogEntry>(filteredLogs, searchCriteria.Total, size);
        }

        /// <summary>
        /// Appends the root.
        /// </summary>
        /// <param name="xmlData">The XML data.</param>
        /// <returns></returns>
        public static string AppendRoot(string xmlData)
        {
            return string.Format("<root xmlns:{0}=\"{0}\">{1}</root>", DefaultNamespace, xmlData);
        }

        /// <summary>
        /// Converts the timestamp.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>The local time for the specified timestamp</returns>
        public DateTime ConvertTimestamp(double timestamp)
        {
            return this.BaseDate.AddMilliseconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Loads the logs.
        /// </summary>
        /// <param name="logs">The log list to load into.</param>
        /// <param name="fileName">Name of the file.</param>
        public void LoadLogs(IList<LogEntry> logs, string fileName)
        {
            int retries = 10;
            while (retries-- > 0)
            {
                try
                {
                    string xmlData = File.ReadAllText(fileName);
                    this.LoadLogsFromXml(logs, LogLoader.AppendRoot(xmlData), true);
                    break;
                }
                catch (IOException)
                {
                    // If there is a lock on the file due to a current log write, retry after a few moments
                    Thread.Sleep(500);
                }
            }

            if (retries <= 0)
            {
                throw new IOException("Could not get exclusive access to " + fileName);
            }
        }

        /// <summary>
        /// Loads the logs from XML.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="xmlData">The XML data.</param>
        /// <param name="sortDescending">Whether logs should be sorted descending.</param>
        internal void LoadLogsFromXml(IList<LogEntry> target, string xmlData, bool sortDescending)
        {
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                return;
            }

            XDocument doc = XDocument.Parse(xmlData);
            XNamespace ns = DefaultNamespace;
            int itemIndex = 1;

            foreach (XElement eventElement in doc.Root.Elements(ns + "event"))
            {
                LogEntry entry = new LogEntry
                {
                    Item = itemIndex,
                    TimeStamp = this.ConvertTimestamp(double.Parse(eventElement.Attribute("timestamp").Value, CultureInfo.InvariantCulture)),
                    Thread = eventElement.Attribute("thread").Value,
                    Level = eventElement.Attribute("level").Value
                };

                foreach (XElement child in eventElement.Elements())
                {
                    if (child.Name == (ns + "message"))
                    {
                        entry.Message = child.Value;
                    }
                    else if (child.Name == (ns + "properties"))
                    {
                        foreach (XElement property in child.Elements(ns + "data"))
                        {
                            var attributeName = property.Attribute("name").Value;
                            var value = property.Attribute("value").Value;
                            switch (attributeName)
                            {
                                case "log4jmachinename":
                                    entry.MachineName = value;
                                    break;

                                case "log4japp":
                                    entry.App = value;
                                    break;

                                case "log4net:UserName":
                                    entry.UserName = value;
                                    break;

                                case "log4net:HostName":
                                    entry.HostName = value;
                                    break;
                            }
                        }
                    }
                    else if (child.Name == (ns + "throwable"))
                    {
                        entry.Throwable = child.Value;
                    }
                    else if (child.Name == (ns + "locationInfo"))
                    {
                        entry.Class = child.Attribute("class").Value;
                        entry.Method = child.Attribute("method").Value;
                        entry.File = child.Attribute("file").Value;
                        entry.Line = child.Attribute("line").Value;
                    }
                }

                if (sortDescending)
                {
                    target.Insert(0, entry);
                }
                else
                {
                    target.Add(entry);
                }

                itemIndex++;
            }
        }

        #endregion Methods
    }
}