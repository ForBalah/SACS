using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Email;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.BusinessLayer.BusinessLogic.Validation;
using SACS.BusinessLayer.Factories.Interfaces;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Application
{
    /// <summary>
    /// The default AppManager implemenation
    /// </summary>
    public class AppManager : IAppManager
    {
        #region Fields
        
        private readonly ILog _log = LogManager.GetLogger(typeof(AppManager)); // TODO: inject
        private IDaoFactory _daoFactory;
        private EmailProvider _emailer;
        private ServiceAppProcessCollection _ServiceAppProcesses;
        private IServiceAppSchedulingService _schedulingService;
        private IServiceAppProcessFactory _serviceAppFactory;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppManager"/> class.
        /// </summary>
        /// <param name="serviceAppFactory">The factory that creates <see cref="ServiceAppProcess"/>.</param>
        /// <param name="schedulingService">The scheduling service.</param>
        /// <param name="emailer">The email provider.</param>
        /// <param name="daoFactory">The DAO factory.</param>
        public AppManager(IServiceAppProcessFactory serviceAppFactory, IServiceAppSchedulingService schedulingService, EmailProvider emailer, IDaoFactory daoFactory)
        {
            _ServiceAppProcesses = new ServiceAppProcessCollection(new ServiceAppProcessComparer());
            _serviceAppFactory = serviceAppFactory;
            _schedulingService = schedulingService;
            _emailer = emailer;
            _daoFactory = daoFactory;
        }

        #endregion Constructors and Destructors

        #region Properties

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
                return ServiceAppProcesses.Select(dom => dom.ServiceApp).ToList();
            }
        }

        /// <summary>
        /// Gets the service application domains.
        /// </summary>
        /// <value>
        /// The service application domains.
        /// </value>
        public ServiceAppProcessCollection ServiceAppProcesses
        {
            get
            {
                return _ServiceAppProcesses;
            }
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Error event of the ServiceAppProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServiceAppErrorEventArgs"/> instance containing the event data.</param>
        private void ServiceAppProcess_Error(object sender, ServiceAppErrorEventArgs e)
        {
            EmailHelper.SendSupportEmail(_emailer, e.Exception, e.Name);
        }

        /// <summary>
        /// Handles the Executed event of the ServiceAppProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServiceAppSuccessEventArgs"/> instance containing the event data.</param>
        private void ServiceAppProcess_Executed(object sender, ServiceAppSuccessEventArgs e)
        {
            if (e.SendSuccessNotification)
            {
                EmailHelper.SendSuccessEmail(_emailer, e.Name, e.Environment, e.FinishTime);
            }
        }

        /// <summary>
        /// Handles the Started event of the ServiceAppProcess object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ServiceAppProcess_Started(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            ServiceAppProcess process = sender as ServiceAppProcess;

            if (sender != null)
            {
                if (process.IsProcessRunning)
                {
                    using (var dao = _daoFactory.Create<IServiceAppDao>())
                    {
                        dao.RecordServiceAppStart(process.ServiceApp.Name);
                    }

                    if (process.ServiceApp.StartupTypeEnum == StartupType.Automatic)
                    {
                        errorMessage = _schedulingService.ScheduleServiceApp(process);
                    }
                }
                else
                {
                    errorMessage = string.Format("Failed to initialize ServiceApp '{0}'", process.ServiceApp.Name);
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                this._log.Warn(errorMessage);
            }
        }

        /// <summary>
        /// Handles the Performance event of the ServiceAppProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServiceAppPerformanceEventArgs"/> instance containing the event data.</param>
        private void ServiceAppProcess_Performance(object sender, ServiceAppPerformanceEventArgs e)
        {
            try
            {
                using (var dao = _daoFactory.Create<IServiceAppDao>())
                {
                    dao.RecordPerfromance(e.Name, e.AppPerformance);
                }
            }
            catch
            {
                // TODO: do something about performance logging failing.
                // throw;
            }
        }

        /// <summary>
        /// Handles the Stopped event of the ServiceAppProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServiceAppStopEventArgs"/> instance containing the event data.</param>
        private void ServiceAppProcess_Stopped(object sender, ServiceAppStopEventArgs e)
        {
            _schedulingService.RemoveJob(e.Name);

            if (e.HasError)
            {
                EmailHelper.SendSupportEmail(_emailer, null, e.Name);
            }
        }

        #endregion Event Handlers

        #region Methods

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
                string errorMessage = this.SyncServiceApp(app, dao, null);

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
            ServiceAppProcess process = this.ServiceAppProcesses[appName];

            if (process == null)
            {
                var e = new IndexOutOfRangeException(string.Format("ServiceApp '{0}' could not be found to initialize.", appName));
                this._log.Error("Error in InitializeServiceApp", e);
                throw e;
            }
            else if (process.IsProcessRunning)
            {
                var e = new InvalidOperationException(string.Format("ServiceApp '{0}' must be stopped before it can be reinitialized.", appName));
                this._log.Error("Error in InitializeServiceApp", e);
                throw e;
            }

            if (process.ServiceApp.StartupTypeEnum != StartupType.Disabled)
            {
                process.Start();
                return string.Empty;
            }
            else
            {
                return string.Format("ServiceApp '{0}' is disabled", appName);
            }
        }

        /// <summary>
        /// Stops all running service apps.
        /// </summary>
        /// <param name="dao">The ServiceApp DAO</param>
        /// <param name="isExiting">Set to <c>true</c> if the service is in the middle of exiting.</param>
        public void StopAllServiceApps(IServiceAppDao dao, bool isExiting)
        {
            foreach (var dom in this.ServiceAppProcesses.Where(d => d.IsProcessRunning))
            {
                this.StopServiceApp(dom.ServiceApp.Name, dao, isExiting);
            }
        }

        /// <summary>
        /// Stops the specified service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="isExiting">Set to <c>true</c> if the service is in the middle of exiting.</param>
        /// <exception cref="System.IndexOutOfRangeException">The app name could not be found.</exception>
        public void StopServiceApp(string appName, IServiceAppDao dao, bool isExiting)
        {
            ServiceAppProcess process = this.ServiceAppProcesses[appName];

            if (process == null)
            {
                var e = new IndexOutOfRangeException(string.Format("appName '{0}' could not be found to stop.", appName));
                this._log.Error("Error in StopServiceApp", e);
            }
            else
            {
                process.Stop(isExiting);
                _schedulingService.RemoveJob(appName);
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
        public string SyncServiceApp(ServiceApp app, IServiceAppDao dao, IAppListDao appListDao)
        {
            string errorMessage = string.Empty;
            this.RemoveServiceApp(app.Name, dao);

            ServiceAppProcess process = _serviceAppFactory.CreateServiceAppProcess(app, this._log);
            try
            {
                this.ServiceAppProcesses.Add(process);
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

                process.Started += ServiceAppProcess_Started;
                process.Error += ServiceAppProcess_Error;
                process.Executed += ServiceAppProcess_Executed;
                process.Performance += ServiceAppProcess_Performance;
                process.Stopped += ServiceAppProcess_Stopped;
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
            ServiceAppProcess process = ServiceAppProcesses[appName];

            if (process != null)
            {
                if (process.IsProcessRunning)
                {
                    throw new InvalidOperationException(string.Format("ServiceApp '{0}' must be stopped before it can be updated or removed.", appName));
                }

                ServiceAppProcesses.Remove(process);

                // This will prevent leaks
                process.Started -= ServiceAppProcess_Started;
                process.Error -= ServiceAppProcess_Error;
                process.Executed -= ServiceAppProcess_Executed;
                process.Performance -= ServiceAppProcess_Performance;
                process.Stopped -= ServiceAppProcess_Stopped;
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
        public void RunServiceApp(string appName)
        {
            string errorMessage = string.Empty;
            ServiceAppProcess process = this.ServiceAppProcesses[appName];

            if (process == null)
            {
                throw new IndexOutOfRangeException(string.Format("ServiceApp '{0}' could not be found to execute.", appName));
            }
            else if (!process.IsProcessRunning)
            {
                throw new InvalidOperationException(string.Format("ServiceApp '{0}' must be started before it can be run.", appName));
            }

            // run the service app as if the timer elapsed
            process.ExecuteServiceApp();
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
            // TODO: move validation outside of here (breaks SRP)
            ServiceAppValidator validator = new ServiceAppValidator();

            if (serviceApp == null)
            {
                return "Cannot update null service app";
            }

            if (!validator.ValidateServiceApp(serviceApp))
            {
                return string.Format("Service app to update is invalid. Reason: {0}", string.Join(", ", validator.ErrorMessages));
            }

            return this.SyncServiceApp(serviceApp, dao, appListDao);
        }

        #endregion Methods
    }
}