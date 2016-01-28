﻿using System;
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

        private static EmailProvider _emailer = new EmailProvider();
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceContainer));
        private readonly IServiceAppSchedulingService _schedulingService;
        private readonly WebAPIComponent _webApiComponent;
        private readonly IAppManager _appManager = AppManager.Current;
        private bool _isStarted = false; // for debugging

        #endregion fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.WindowsService.Components.ServiceContainer" /> class.
        /// </summary>
        /// <param name="schedulingService">The custom built scheduling service.</param>
        /// <param name="webApiComponent">The Communication component.</param>
        public ServiceContainer(IServiceAppSchedulingService schedulingService, WebAPIComponent webApiComponent)
        {
            this._schedulingService = schedulingService;
            this._webApiComponent = webApiComponent;
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
                    EmailHelper.SendSupportEmail(_emailer, ex, null);
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
            SystemMonitor.AddToScheduler(this._schedulingService);
            this._schedulingService.Start();

            // TODO: Not comfortable having this here. Can it be moved out?
            this._appManager.SchedulingService = this._schedulingService;
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
                AppManager.Current.StopAllServiceApps(serviceAppDao);
            }
        }

        #endregion Methods
    }
}