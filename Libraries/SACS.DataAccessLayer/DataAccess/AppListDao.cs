using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NCrontab;
using SACS.Common.Configuration;
using SACS.Common.Enums;
using SACS.Common.Helpers;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Extensions;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// The DAO for accessing the App List
    /// </summary>
    public class AppListDao : DaoBase, IAppListDao
    {
        #region Fields

        private XDocument _AppListDoc;
        private bool _isLoadFromFile = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AppListDao"/> class from being created.
        /// </summary>
        private AppListDao()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AppListDao"/> class from being created.
        /// </summary>
        /// <param name="xml">The XML.</param>
        private AppListDao(string xml)
        {
            this._AppListDoc = XDocument.Parse(xml);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the application list document.
        /// </summary>
        /// <value>
        /// The application list document.
        /// </value>
        protected XDocument AppListDoc
        {
            get
            {
                if (this._AppListDoc == null)
                {
                    // TODO: move this out of the file so that it can be injected in by a factory
                    string path = Path.GetFullPath(ApplicationSettings.Current.AppListLocation);
                    this._AppListDoc = XDocument.Load(path);
                    this._isLoadFromFile = true;
                }

                return this._AppListDoc;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all the service apps
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceApp> FindAll()
        {
            return this.FindAll(null);
        }

        /// <summary>
        /// Finds all the service apps, filtering by expression
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IEnumerable<ServiceApp> FindAll(Func<ServiceApp, bool> filter)
        {
            var apps = this.AppListDoc.Root.Element("collection").Elements().Select(n => CastToServiceApp(n));
            if (filter == null)
            {
                return apps;
            }

            return apps.Where(filter);
        }

        /// <summary>
        /// Persists the service app.
        /// </summary>
        /// <param name="app">The application.</param>
        public void PersistServiceApp(ServiceApp app)
        {
            var appElement = this.AppListDoc.Root.Element("collection").Elements().FirstOrDefault(e => e.Attribute("name").Value == app.Name);

            if (appElement == null)
            {
                appElement = new XElement("serviceApp");
                this.AppListDoc.Root.Element("collection").Add(appElement);
            }

            bool isBackedUp = this.BackupXmlFile(app, SafeCastToServiceApp(appElement));

            appElement.ReplaceAttributes(
                new XAttribute("name", app.Name),
                new XAttribute("description", app.Description),
                new XAttribute("environment", app.Environment),
                new XAttribute("path", app.Path),
                new XAttribute("entryFile", app.EntryFile),
                new XAttribute("assembly", app.Assembly),
                new XAttribute("startupType", app.StartupType),
                new XAttribute("configFilePath", app.ConfigFilePath),
                new XAttribute("schedule", app.Schedule),
                new XAttribute("username", app.Username),
                new XAttribute("password", app.Password));

            if (this._isLoadFromFile && isBackedUp)
            {
                this.AppListDoc.Save(Path.GetFullPath(ApplicationSettings.Current.AppListLocation));
            }
        }

        /// <summary>
        /// Deletes the service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void DeleteServiceApp(string appName)
        {
            var appElement = this.AppListDoc.Root.Element("collection").Elements().FirstOrDefault(e =>
                e.Attribute("name").Value == appName);

            if (appElement != null)
            {
               appElement.Remove();
               this.BackupXmlFile(null, null);

               if (this._isLoadFromFile)
               {
                   this.AppListDoc.Save(Path.GetFullPath(ApplicationSettings.Current.AppListLocation));
               }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._AppListDoc != null)
            {
                this._AppListDoc = null;
            }
        }

        /// <summary>
        /// Safe casts to service app.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static ServiceApp SafeCastToServiceApp(XElement element)
        {
            try
            {
                return CastToServiceApp(element);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Casts to service app.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        internal static ServiceApp CastToServiceApp(XElement element)
        {
            if (element == null || element.Name != "serviceApp")
            {
                return null;
            }

            ServiceApp app = new ServiceApp
            {
                Name = element.Attribute("name").Value,
                Description = element.Attribute("description").Value,
                Environment = element.Attribute("environment").Value,
                Path = element.Attribute("path").Value,
                EntryFile = element.Attribute("entryFile").Value,
                Assembly = element.Attribute("assembly").Value,
                StartupTypeEnum = EnumHelper.Parse<StartupType>(element.Attribute("startupType").Value),
                ConfigFilePath = element.Attribute("configFilePath").Value,
                Schedule = element.Attribute("schedule").Value,
                Username = element.Attribute("username").EmptyIfNull("username").Value,
                Password = element.Attribute("password").EmptyIfNull("password").Value
            };

            return app;
        }

        /// <summary>
        /// Backups the XML file, if the ServiceApps are different
        /// </summary>
        /// <param name="compare1">The compare1.</param>
        /// <param name="compare2">The compare2.</param>
        /// <returns></returns>
        private bool BackupXmlFile(ServiceApp compare1, ServiceApp compare2)
        {
            if (compare1 == null || compare2 == null || !compare1.Equals(compare2))
            {
                // if different, create backup of xml file
                if (this._isLoadFromFile)
                {
                    string original = Path.GetFullPath(ApplicationSettings.Current.AppListLocation);
                    string backupFile = string.Format(
                        "{0} {1}{2}",
                        Path.Combine(Path.GetDirectoryName(original), Path.GetFileNameWithoutExtension(original)),
                        DateTime.Now.ToString("yyyyMMdd hhmm"),
                        Path.GetExtension(original));

                    this.AppListDoc.Save(Path.GetFullPath(backupFile));
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}
