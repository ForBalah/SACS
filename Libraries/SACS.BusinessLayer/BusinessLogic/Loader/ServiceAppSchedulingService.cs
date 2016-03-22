using System;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.Common.Enums;
using SACS.Scheduler.Service;

namespace SACS.BusinessLayer.BusinessLogic.Loader
{
    /// <summary>
    /// Class that facilitates the scheduling of serviceApps
    /// </summary>
    public class ServiceAppSchedulingService : SchedulingService, IServiceAppSchedulingService
    {
        /// <summary>
        /// Schedules the service app process.
        /// </summary>
        /// <param name="process">The process to schedule.</param>
        /// <returns>
        /// Any errors that occured when trying to create the schedule.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Cannot schedule an uninitialized ServiceApp.</exception>
        public string ScheduleServiceApp(ServiceAppProcess process)
        {
            if (!process.IsProcessRunning)
            {
                throw new InvalidOperationException("Cannot schedule a service app that has not been started.");
            }

            string errorMessage = string.Empty;
            string appName = process.ServiceApp.Name;

            if (process.ServiceApp.StartupTypeEnum == StartupType.Automatic)
            {
                if (string.IsNullOrEmpty(process.ServiceApp.Schedule))
                {
                    process.AddMessage(ServiceAppMessages.InvalidSchedule, ServiceAppState.Error);
                    errorMessage = string.Format("Schedule for service app '{0}' was not defined", appName);
                }
                else if (!HasJob(appName))
                {
                    AddJob(appName, process.ServiceApp.Schedule, () => { process.ExecuteServiceApp(); });
                }
            }
            else if (process.ServiceApp.StartupTypeEnum != StartupType.Manual)
            {
                errorMessage = string.Format("Cannot start '{0}'. The service app is disabled. Please enable the service app first before scheduling.", appName);
            }

            return errorMessage;
        }
    }
}