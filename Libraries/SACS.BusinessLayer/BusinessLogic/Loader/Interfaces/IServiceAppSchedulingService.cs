using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.Scheduler.Service;

namespace SACS.BusinessLayer.BusinessLogic.Loader.Interfaces
{
    /// <summary>
    /// The scheduling service for Service Apps
    /// </summary>
    public interface IServiceAppSchedulingService : ISchedulingService
    {
        /// <summary>
        /// Schedules the service application domain.
        /// </summary>
        /// <param name="domain">The scheduling service.</param>
        /// <param name="dao">The DAO.</param>
        /// <returns>
        /// Any errors that occured when trying to create the schedule.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// The ServiceAppDomain cannot schedule an uninitialized ServiceApp.
        /// or
        /// The ServiceAppDomain has already been marked to unload. Create a new ServiceAppDomain.
        /// </exception>
        string ScheduleServiceApp(ServiceAppDomain domain, IServiceAppDao dao);
    }
}
