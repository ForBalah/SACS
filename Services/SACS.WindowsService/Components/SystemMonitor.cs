﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
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
    internal class SystemMonitor
    {
        private static SystemMonitor monitor = new SystemMonitor();
        private static ILog _log = LogManager.GetLogger(typeof(SystemMonitor));
        private static DateTime? _startServiceTime;
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

        /// <summary>
        /// Kicks off the running of the schedule monitor.
        /// </summary>
        /// <param name="service">The schedulign service to use.</param>
        public static void AddToScheduler(ISchedulingService service)
        {
            service.AddJob(
                typeof(SystemMonitor).Name,
                ConfigurationManager.AppSettings[Constants.MonitorSchedule],
                () => { monitor.RecordPerformance(); });
        }

        /// <summary>
        /// Records the performance of the SAC system
        /// </summary>
        public void RecordPerformance()
        {
            TimeSpan monitorDifference = new TimeSpan(0);
            if (_startServiceTime == null)
            {
                _startServiceTime = DateTime.Now;
            }

            monitorDifference = DateTime.Now - (_startServiceTime ?? DateTime.Now);

            decimal? cpuValue = Math.Floor((decimal)cpuCounter.NextValue() / (decimal)Environment.ProcessorCount);
            decimal ramValue = (decimal)ramCounter.NextValue() / 1024 / 1024;
            string message = "Monitor reports as running";

            _log.Debug(
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
