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
        /// The default base date
        /// </summary>
        public static DateTime DefaultDate = new DateTime(1970, 1, 1);

        public LogLoader(DateTime baseDate)
        {
            this.BaseDate = baseDate;
        }

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

            /*
               
                                    case ("log4j:throwable"):
                                        {
                                            logentry.Throwable = oXmlTextReader.ReadString();
                                            break;
                                        }
                                    case ("log4j:locationInfo"):
                                        {
                                            logentry.Class = oXmlTextReader.GetAttribute("class");
                                            logentry.Method = oXmlTextReader.GetAttribute("method");
                                            logentry.File = oXmlTextReader.GetAttribute("file");
                                            logentry.Line = oXmlTextReader.GetAttribute("line");
                                            break;
                                        }
                                }
                            }
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        _Entries.Add(logentry);
                        iIndex++;

                        #region Show Counts
                        ////////////////////////////////////////////////////////////////////////////////
                        int ErrorCount =
                        (
                            from entry in Entries
                            where entry.Level == "ERROR"
                            select entry
                        ).Count();

                        if (ErrorCount > 0)
                        {
                            labelErrorCount.Content = string.Format("{0:#,#}  ", ErrorCount);
                            labelErrorCount.Visibility = Visibility.Visible;
                            imageError.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelErrorCount.Visibility = Visibility.Hidden;
                            imageError.Visibility = Visibility.Hidden;
                        }

                        int InfoCount =
                        (
                            from entry in Entries
                            where entry.Level == "INFO"
                            select entry
                        ).Count();

                        if (InfoCount > 0)
                        {
                            labelInfoCount.Content = string.Format("{0:#,#}  ", InfoCount);
                            labelInfoCount.Visibility = Visibility.Visible;
                            imageInfo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelInfoCount.Visibility = Visibility.Hidden;
                            imageInfo.Visibility = Visibility.Hidden;
                        }

                        int WarnCount =
                        (
                            from entry in Entries
                            where entry.Level == "WARN"
                            select entry
                        ).Count();

                        if (WarnCount > 0)
                        {
                            labelWarnCount.Content = string.Format("{0:#,#}  ", WarnCount);
                            labelWarnCount.Visibility = Visibility.Visible;
                            imageWarn.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelWarnCount.Visibility = Visibility.Hidden;
                            imageWarn.Visibility = Visibility.Hidden;
                        }

                        int DebugCount =
                        (
                            from entry in Entries
                            where entry.Level == "DEBUG"
                            select entry
                        ).Count();

                        if (DebugCount > 0)
                        {
                            labelDebugCount.Content = string.Format("{0:#,#}  ", DebugCount);
                            labelDebugCount.Visibility = Visibility.Visible;
                            imageDebug.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelDebugCount.Visibility = Visibility.Hidden;
                            labelDebugCount.Visibility = Visibility.Hidden;
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.listView1.ItemsSource = _Entries;
            */
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            ////////////////////////////////////////////////////
            /*
            DateTime minDate = new DateTime(1970, 1, 1);
            string sXml = string.Empty;
            string sBuffer = string.Empty;
            int iIndex = 1;

            try
            {
                FileStream oFileStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                StreamReader oStreamReader = new StreamReader(oFileStream);
                sBuffer = string.Format("<root>{0}</root>", oStreamReader.ReadToEnd());
                oStreamReader.Close();
                oFileStream.Close();

                #region Read File Buffer
                StringReader oStringReader = new StringReader(sBuffer);
                XmlTextReader oXmlTextReader = new XmlTextReader(oStringReader);
                oXmlTextReader.Namespaces = false;
                while (oXmlTextReader.Read())
                {
                    if ((oXmlTextReader.NodeType == XmlNodeType.Element) && (oXmlTextReader.Name == "log4j:event"))
                    {
                        LogEntry logentry = new LogEntry();

                        logentry.Item = iIndex;

                        double dSeconds = Convert.ToDouble(oXmlTextReader.GetAttribute("timestamp"));
                        logentry.TimeStamp = minDate.AddMilliseconds(dSeconds).ToLocalTime();
                        logentry.Thread = oXmlTextReader.GetAttribute("thread");

                        #region get level
                        ////////////////////////////////////////////////////////////////////////////////
                        logentry.Level = oXmlTextReader.GetAttribute("level");
                        switch (logentry.Level)
                        {
                            case "ERROR":
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.ERROR);
                                    break;
                                }
                            case "INFO":
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.INFO);
                                    break;
                                }
                            case "DEBUG":
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.DEBUG);
                                    break;
                                }
                            case "WARN":
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.WARN);
                                    break;
                                }
                            case "FATAL":
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.FATAL);
                                    break;
                                }
                            default:
                                {
                                    logentry.Image = LogEntry.Images(LogEntry.IMAGE_TYPE.CUSTOM);
                                    break;
                                }
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        #region read xml
                        ////////////////////////////////////////////////////////////////////////////////
                        while (oXmlTextReader.Read())
                        {
                            if (oXmlTextReader.Name == "log4j:event")   // end element
                                break;
                            else
                            {
                                switch (oXmlTextReader.Name)
                                {
                                    case ("log4j:message"):
                                        {
                                            logentry.Message = oXmlTextReader.ReadString();
                                            break;
                                        }
                                    case ("log4j:data"):
                                        {
                                            switch (oXmlTextReader.GetAttribute("name"))
                                            {
                                                case ("log4jmachinename"):
                                                    {
                                                        logentry.MachineName = oXmlTextReader.GetAttribute("value");
                                                        break;
                                                    }
                                                case ("log4net:HostName"):
                                                    {
                                                        logentry.HostName = oXmlTextReader.GetAttribute("value");
                                                        break;
                                                    }
                                                case ("log4net:UserName"):
                                                    {
                                                        logentry.UserName = oXmlTextReader.GetAttribute("value");
                                                        break;
                                                    }
                                                case ("log4japp"):
                                                    {
                                                        logentry.App = oXmlTextReader.GetAttribute("value");
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case ("log4j:throwable"):
                                        {
                                            logentry.Throwable = oXmlTextReader.ReadString();
                                            break;
                                        }
                                    case ("log4j:locationInfo"):
                                        {
                                            logentry.Class = oXmlTextReader.GetAttribute("class");
                                            logentry.Method = oXmlTextReader.GetAttribute("method");
                                            logentry.File = oXmlTextReader.GetAttribute("file");
                                            logentry.Line = oXmlTextReader.GetAttribute("line");
                                            break;
                                        }
                                }
                            }
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion

                        _Entries.Add(logentry);
                        iIndex++;

                        #region Show Counts
                        ////////////////////////////////////////////////////////////////////////////////
                        int ErrorCount =
                        (
                            from entry in Entries
                            where entry.Level == "ERROR"
                            select entry
                        ).Count();

                        if (ErrorCount > 0)
                        {
                            labelErrorCount.Content = string.Format("{0:#,#}  ", ErrorCount);
                            labelErrorCount.Visibility = Visibility.Visible;
                            imageError.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelErrorCount.Visibility = Visibility.Hidden;
                            imageError.Visibility = Visibility.Hidden;
                        }

                        int InfoCount =
                        (
                            from entry in Entries
                            where entry.Level == "INFO"
                            select entry
                        ).Count();

                        if (InfoCount > 0)
                        {
                            labelInfoCount.Content = string.Format("{0:#,#}  ", InfoCount);
                            labelInfoCount.Visibility = Visibility.Visible;
                            imageInfo.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelInfoCount.Visibility = Visibility.Hidden;
                            imageInfo.Visibility = Visibility.Hidden;
                        }

                        int WarnCount =
                        (
                            from entry in Entries
                            where entry.Level == "WARN"
                            select entry
                        ).Count();

                        if (WarnCount > 0)
                        {
                            labelWarnCount.Content = string.Format("{0:#,#}  ", WarnCount);
                            labelWarnCount.Visibility = Visibility.Visible;
                            imageWarn.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelWarnCount.Visibility = Visibility.Hidden;
                            imageWarn.Visibility = Visibility.Hidden;
                        }

                        int DebugCount =
                        (
                            from entry in Entries
                            where entry.Level == "DEBUG"
                            select entry
                        ).Count();

                        if (DebugCount > 0)
                        {
                            labelDebugCount.Content = string.Format("{0:#,#}  ", DebugCount);
                            labelDebugCount.Visibility = Visibility.Visible;
                            imageDebug.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            labelDebugCount.Visibility = Visibility.Hidden;
                            labelDebugCount.Visibility = Visibility.Hidden;
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                        #endregion
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            this.listView1.ItemsSource = _Entries;
            */
        }
    }
}
