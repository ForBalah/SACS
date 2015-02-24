using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.Scheduler.Service;

namespace SACS.BusinessLayer.BusinessLogic.Loader
{
    /// <summary>
    /// Class that facilitates the scheduling of serviceApps
    /// </summary>
    public class ServiceAppSchedulingService : SchedulingService, IServiceAppSchedulingService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceAppSchedulingService));

        /// <summary>
        /// Schedules the service application domain.
        /// </summary>
        /// <param name="schedulingService">The scheduling service.</param>
        /// <param name="dao">The DAO.</param>
        /// <returns>
        /// Any errors that occured when trying to create the schedule.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// The ServiceAppDomain cannot schedule an uninitialized ServiceApp.
        /// or
        /// The ServiceAppDomain has already been marked to unload. Create a new ServiceAppDomain.
        /// </exception>
        public string ScheduleServiceApp(ServiceAppDomain domain, IServiceAppDao dao)
        {
            switch (domain.CurrentState)
            {
                case ServiceAppState.NotLoaded:
                    throw new InvalidOperationException("The ServiceAppDomain cannot schedule an uninitialized ServiceApp.");
                case ServiceAppState.Unloading:
                    throw new InvalidOperationException("The ServiceAppDomain cannot schedule a ServiceApp that is currently unloading.");
            }

            string errorMessage = string.Empty;
            string appName = domain.ServiceApp.Name;

            if (string.IsNullOrEmpty(domain.ServiceApp.Schedule))
            {
                domain.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.InvalidSchedule, ServiceAppState.Error));
                this._log.Warn(string.Format("{0} ServiceApp schedule was not provided.", appName));
                errorMessage = string.Format("Schedule for '{0}' was not defined", appName);
            }
            else if (domain.ServiceApp.StartupTypeEnum == Common.Enums.StartupType.Automatic ||
                domain.ServiceApp.StartupTypeEnum == Common.Enums.StartupType.Manual)
            {
                if (!this.HasJob(appName))
                {
                    this.AddJob(appName, domain.ServiceApp.Schedule, () => { domain.RunServiceApp(); });
                }

                dao.RecordServiceAppStart(domain.ServiceApp.Name);
            }
            else
            {
                errorMessage = string.Format("Cannot start '{0}'. The service app is disabled. Please enable the service app first before scheduling.", appName);
            }

            return errorMessage;
        }
    }
}
