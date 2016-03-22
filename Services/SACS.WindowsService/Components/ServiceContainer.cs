using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Application;
using SACS.BusinessLayer.BusinessLogic.Email;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.WindowsService.Common;

namespace SACS.WindowsService.Components
{
    /// <summary>
    /// The main container class for managing the service.
    /// </summary>
    public class ServiceContainer
    {
        #region fields

        private static EmailProvider Emailer = new SmtpEmailProvider(LogManager.GetLogger(typeof(EmailProvider))); // TODO: maybe move to DI?
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceContainer));
        private readonly IServiceAppSchedulingService _schedulingService;
        private readonly WebAPIComponent _webApiComponent;
        private readonly IAppManager _appManager;
        private readonly SystemMonitor _systemMonitor;
        private bool _isStarted = false; // for debugging

        #endregion fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer" /> class.
        /// </summary>
        /// <param name="appManager">The service application manager component.</param>
        /// <param name="schedulingService">The scheduling service component.</param>
        /// <param name="webApiComponent">The communication component.</param>
        /// <param name="systemMonitor">The system monitor component.</param>
        public ServiceContainer(IAppManager appManager, IServiceAppSchedulingService schedulingService, WebAPIComponent webApiComponent, SystemMonitor systemMonitor)
        {
            _appManager = appManager;
            _schedulingService = schedulingService;
            _webApiComponent = webApiComponent;
            _systemMonitor = systemMonitor;
        }

        #endregion Constructors and Destructors

        #region Methods

        /// <summary>
        /// Starts the windows service
        /// </summary>
        public void Start()
        {
            this.StartScheduleMonitor();
            this.StartCommunicationService();
            this.StartServiceApps();
            this._isStarted = true;
        }

        /// <summary>
        /// Stops the windows service.
        /// </summary>
        public void Stop()
        {
            try
            {
                this.StopServiceApps();
                this.StopCommunicationService();
                this.StopScheduleMonitor();
            }
            catch (Exception ex)
            {
                // We don't want to interfere with the service stopping so we try to handle
                // the exception as best as possible, then let TopShelf shutdown the service
                try
                {
                    this._log.Fatal("Fatal exception stopping SACS", ex);
                    EmailHelper.SendSupportEmail(Emailer, ex, null);
                }
                finally
                {
                    this._isStarted = false;
                }
            }
        }

        /// <summary>
        /// Starts the service in the communication component
        /// </summary>
        private void StartCommunicationService()
        {
            this._webApiComponent.Start();
        }

        /// <summary>
        /// Loads and schedules the service apps.
        /// </summary>
        private void StartServiceApps()
        {
            // load all the services into the container
            IList<string> errors = new List<string>();

            // TODO: inject via abstract factory
            using (IServiceAppDao serviceAppDao = DaoFactory.Create<IServiceAppDao, ServiceAppDao>())
            {
                IAppListDao appListDao = DaoFactory.Create<IAppListDao, AppListDao>();
                this._appManager.InitializeAllServiceApps(appListDao.FindAll(), serviceAppDao, errors);
            }

            errors.ToList().ForEach(error =>
            {
                this._log.Warn(error);
            });
        }

        /// <summary>
        /// Starts the health monitor (using NCron)
        /// </summary>
        private void StartScheduleMonitor()
        {
            this._systemMonitor.AddToScheduler(this._schedulingService);
            this._schedulingService.Start();
            this._log.Info(string.Format("The schedule monitor, and scheduling service have successfully started. Monitor schedule set at: {0}", ConfigurationManager.AppSettings[Constants.MonitorSchedule]));
        }

        /// <summary>
        /// Stops the service in the communication component
        /// </summary>
        private void StopCommunicationService()
        {
            this._webApiComponent.Stop();
        }

        /// <summary>
        /// Stops the schedule monitor and all scheduled jobs
        /// </summary>
        private void StopScheduleMonitor()
        {
            if (this._schedulingService != null)
            {
                this._schedulingService.Stop();
            }
        }

        /// <summary>
        /// Stops the service apps.
        /// </summary>
        private void StopServiceApps()
        {
            using (IServiceAppDao serviceAppDao = DaoFactory.Create<IServiceAppDao, ServiceAppDao>())
            {
                _appManager.StopAllServiceApps(serviceAppDao, true);
            }
        }

        #endregion Methods
    }
}