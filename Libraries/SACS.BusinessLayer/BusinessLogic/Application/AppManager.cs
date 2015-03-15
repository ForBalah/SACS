using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.BusinessLayer.BusinessLogic.Validation;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Application
{
    /// <summary>
    /// The AppManager implemenation
    /// </summary>
    public class AppManager : IAppManager
    {
        #region Fields

        private readonly ILog _log = LogManager.GetLogger(typeof(AppManager));

        private static object _syncRoot = new object();
        private static IAppManager _Current;

        private ServiceAppDomainCollection _ServiceAppDomains;
        private IServiceAppSchedulingService _SchedulingService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AppManager"/> class from being created.
        /// </summary>
        private AppManager()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current AppManager
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static IAppManager Current
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_Current == null)
                    {
                        _Current = new AppManager();
                    }

                    return _Current;
                }
            }

            set
            {
                _Current = value;
            }
        }

        /// <summary>
        /// Gets or sets the scheduling service.
        /// </summary>
        /// <value>
        /// The scheduling service.
        /// </value>
        public IServiceAppSchedulingService SchedulingService
        {
            get
            {
                return this._SchedulingService;
            }

            set
            {
                this._SchedulingService = value;
            }
        }

        /// <summary>
        /// Gets the loaded service apps.
        /// </summary>
        /// <value>
        /// The service apps.
        /// </value>
        public IList<ServiceApp> ServiceApps
        {
            get
            {
                return this.ServiceAppDomains.Select(dom => dom.ServiceApp).ToList();
            }
        }

        /// <summary>
        /// Gets the service application domains.
        /// </summary>
        /// <value>
        /// The service application domains.
        /// </value>
        protected ServiceAppDomainCollection ServiceAppDomains
        {
            get
            {
                if (this._ServiceAppDomains == null)
                {
                    this._ServiceAppDomains = new ServiceAppDomainCollection(new ServiceAppDomainComparer());
                }

                return this._ServiceAppDomains;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the list of app domains that have been loaded.
        /// </summary>
        /// <returns></returns>
        public IList<AppDomain> GetDomains()
        {
            return this.ServiceAppDomains.Select(d => d.AppDomain).ToList();
        }

        /// <summary>
        /// Loads all the service apps and schedules them.
        /// </summary>
        /// <param name="appList">The application list.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="errorList">The error list.</param>
        public void InitializeAllServiceApps(IEnumerable<ServiceApp> appList, IServiceAppDao dao, IList<string> errorList)
        {
            foreach (ServiceApp app in appList)
            {
                string errorMessage = this.PersistServiceApp(app, dao, null);

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = this.InitializeServiceApp(app.Name, dao);
                }

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorList.Add(errorMessage);
                }
            }
        }

        /// <summary>
        /// Loads the service app which should be added to the pool of service apps.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <returns>A string containing an error message if a recoverable error occured.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The ServiceApp could not be found in the container.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp is not in a stopped state.</exception>
        public string InitializeServiceApp(string appName, IServiceAppDao dao)
        {
            string errorMessage = string.Empty;
            ServiceAppDomain domain = this.ServiceAppDomains[appName];

            if (domain == null)
            {
                var e = new IndexOutOfRangeException(string.Format("ServiceApp '{0}' could not be found to initialize.", appName));
                this._log.Error("Error in InitializeServiceApp", e);
                throw e;
            }
            else if (domain.CurrentState != ServiceAppState.NotLoaded &&
                domain.CurrentState != ServiceAppState.Unknown &&
                domain.CurrentState != ServiceAppState.Error)
            {
                var e = new InvalidOperationException(string.Format("ServiceApp '{0}' must be stopped before it can be reinitialized.", appName));
                this._log.Error("Error in InitializeServiceApp", e);
                throw e;
            }

            if (domain.ServiceApp.StartupTypeEnum != StartupType.Disabled)
            {
                domain.Initialize();

                if (domain.CurrentState == ServiceAppState.Initialized)
                {
                    if (domain.ServiceApp.StartupTypeEnum == Common.Enums.StartupType.Automatic)
                    {
                        errorMessage = this.SchedulingService.ScheduleServiceApp(domain, dao);
                    }
                }
                else
                {
                    errorMessage = string.Format("Failed to initialize ServiceApp '{0}'", appName);
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                this._log.Warn(errorMessage);
            }

            return errorMessage;
        }

        /// <summary>
        /// Stops all running service apps.
        /// </summary>
        /// <param name="dao">The ServiceApp DAO</param>
        public void StopAllServiceApps(IServiceAppDao dao)
        {
            foreach (var dom in this.ServiceAppDomains.Where(d => d.CurrentState != ServiceAppState.Unloading))
            {
                this.StopServiceApp(dom.ServiceApp.Name, dao);
            }
        }

        /// <summary>
        /// Stops the specified service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        public void StopServiceApp(string appName, IServiceAppDao dao)
        {
            ServiceAppDomain domain = this.ServiceAppDomains[appName];

            if (domain == null)
            {
                var e = new IndexOutOfRangeException(string.Format("appName '{0}' could not be found to stop.", appName));
                this._log.Error("Error in StopServiceApp", e);
            }

            if (domain.CurrentState == ServiceAppState.Initialized || domain.CurrentState == ServiceAppState.Error)
            {
                domain.Unload();
                this.SchedulingService.RemoveJob(appName);
                dao.RecordServiceAppStop(appName);
            }
        }

        /// <summary>
        /// Adds the service app to the container or updates it if it already exists and saves it to the AppList configuration.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <exception cref="System.InvalidOperationException">The service app is added and is still running.</exception>
        public string PersistServiceApp(ServiceApp app, IServiceAppDao dao, IAppListDao appListDao)
        {
            string errorMessage = string.Empty;
            this.RemoveServiceApp(app.Name, dao);

            ServiceAppDomain newDomain = new ServiceAppDomain(app, this._log, new Security.ServiceAppImpersonator());
            try
            {
                this.ServiceAppDomains.Add(newDomain);
                dao.SaveServiceApp(app);
                
                if (appListDao != null)
                {
                    try
                    {
                        appListDao.PersistServiceApp(app);
                    }
                    catch (Exception e)
                    {
                        this._log.Error(string.Format("{0} could not be saved to AppList", app.Name), e);
                        errorMessage = string.Format("ServiceApp '{0}' could not be saved to the configuration.", app.Name);
                    }
                }
            }
            catch (ArgumentException)
            {
                errorMessage = string.Format("ServiceApp '{0}' is already added.", app.Name);
            }

            return errorMessage;
        }

        /// <summary>
        /// Removes the specified service app from the container without removing from the app List.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp was not stopped first.</exception>
        public void RemoveServiceApp(string appName, IServiceAppDao dao)
        {
            this.RemoveServiceApp(appName, dao, null);
        }

        /// <summary>
        /// Removes the specified service app from the container.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        /// <exception cref="System.InvalidOperationException">The ServiceApp was not stopped first.</exception>
        public void RemoveServiceApp(string appName, IServiceAppDao dao, IAppListDao appListDao)
        {
            ServiceAppDomain domain = this.ServiceAppDomains[appName];

            if (domain != null)
            {
                if (domain.CurrentState == ServiceAppState.Initialized)
                {
                    throw new InvalidOperationException(string.Format("ServiceApp '{0}' must be stopped before it can be updated or removed.", appName));
                }

                // An error means that the domain wasn't loaded properly so we can go ahead and remove it
                if (domain.CurrentState == ServiceAppState.Error)
                {
                    domain.Unload();
                    dao.RecordServiceAppStop(appName);
                }

                this.ServiceAppDomains.Remove(domain);
            }

            if (appListDao != null)
            {
                appListDao.DeleteServiceApp(appName);
            }

            if (dao != null)
            {
                dao.DeleteServiceApp(appName);
            }
        }

        /// <summary>
        /// Runs the service application immediately.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        public void RunServiceApp(string appName, IServiceAppDao dao)
        {
            string errorMessage = string.Empty;
            ServiceAppDomain domain = this.ServiceAppDomains[appName];

            if (domain == null)
            {
                throw new IndexOutOfRangeException(string.Format("ServiceApp '{0}' could not be found to execute.", appName));
            }
            else if (domain.CurrentState != ServiceAppState.Initialized)
            {
                throw new InvalidOperationException(string.Format("ServiceApp '{0}' must be initialized before it can be run.", appName));
            }

            // run the service app as if the timer elapsed
            Task runTask = new Task(() =>
            {
                domain.RunServiceApp();
            });

            runTask.Start();
        }

        /// <summary>
        /// Updates the service app in the container or adds a new one to it if it does not exist.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="appListDao">The application list DAO.</param>
        /// <returns>
        /// An error message if there was a problem updating the service app
        /// </returns>
        public string UpdateServiceApp(ServiceApp serviceApp, IServiceAppDao dao, IAppListDao appListDao)
        {
            ServiceAppValidator validator = new ServiceAppValidator();

            if (serviceApp == null)
            {
                return "Cannot update null service app";
            }

            if (!validator.ValidateServiceApp(serviceApp))
            {
                return string.Format("Service app to update is invalid. Reason: {0}", string.Join(", ", validator.ErrorMessages));
            }

            return this.PersistServiceApp(serviceApp, dao, appListDao);
        }

        #endregion
    }
}
