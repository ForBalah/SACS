using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets the base date.
        /// </summary>
        /// <value>
        /// The base date.
        /// </value>
        public DateTime BaseDate { get; private set; }

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
        /// Loads the logs from XML.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="xmlData">The XML data.</param>
        public void LoadLogsFromXml(IList<LogEntry> target, string xmlData)
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
                    TimeStamp = this.ConvertTimestamp(double.Parse(eventElement.Attribute("timestamp").Value)),
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

                target.Add(entry);
                itemIndex++;
            }
        } 

        #endregion
    }
}
