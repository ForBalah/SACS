using System;
using System.Configuration;
using System.Diagnostics;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Application;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.Scheduler.Service;
using SACS.WindowsService.Common;

namespace SACS.WindowsService.Components
{
    /// <summary>
    /// Class that monitors the status of the windows service
    /// </summary>
    public class SystemMonitor
    {
        private static ILog log = LogManager.GetLogger(typeof(SystemMonitor));
        private static DateTime? loggingStartTime;
        private readonly IAppManager _appManager;

        private static PerformanceCounter ramCounter = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "Working Set - Private",
                InstanceName = Process.GetCurrentProcess().ProcessName
            };

        private static PerformanceCounter cpuCounter = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "% Processor Time",
                InstanceName = Process.GetCurrentProcess().ProcessName
            };

        public SystemMonitor(IAppManager appManager)
        {
            _appManager = appManager;
        }

        /// <summary>
        /// Kicks off the running of the schedule monitor.
        /// </summary>
        /// <param name="service">The schedulign service to use.</param>
        public void AddToScheduler(ISchedulingService service)
        {
            var jobName = typeof(SystemMonitor).Name;
            if (!service.HasJob(jobName))
            {
                service.AddJob(
                    typeof(SystemMonitor).Name,
                    ConfigurationManager.AppSettings[Constants.MonitorSchedule],
                    () => { RecordPerformance(); });
            }
        }

        /// <summary>
        /// Records the performance of the SAC system
        /// </summary>
        public void RecordPerformance()
        {
            TimeSpan monitorDifference = new TimeSpan(0);
            if (loggingStartTime == null)
            {
                loggingStartTime = DateTime.Now;
            }

            monitorDifference = DateTime.Now - (loggingStartTime ?? DateTime.Now);

            decimal? cpuValue = Math.Floor((decimal)cpuCounter.NextValue() / (decimal)Environment.ProcessorCount);
            decimal ramValue = ((decimal)ramCounter.NextValue() / 1024m) / 1024m;

            // Add on each service app
            foreach (var process in _appManager.ServiceAppProcesses)
            {
                cpuValue += process.GetCurrentCpuValue() / (decimal)Environment.ProcessorCount;
                ramValue += (process.GetCurrentRamValue() / 1024m) / 1024m;
            }

            string message = "Monitor reports as running";

            log.Debug(
                string.Format(
                    "{0:F} {1}: CPU - {2}%, RAM - {3}MB Remaining, Up-Time: {4}",
                    DateTime.Now,
                    message,
                    cpuValue,
                    ramValue,
                    monitorDifference));

            using (ISystemDao dao = DaoFactory.Create<ISystemDao, SystemDao>())
            {
                dao.LogSystemPerformances(message, cpuValue, ramValue);
            }
        }
    }
}