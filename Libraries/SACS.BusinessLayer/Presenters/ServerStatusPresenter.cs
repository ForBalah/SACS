using System;
using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using SACS.BusinessLayer.Views;
using SACS.Common.Configuration;
using SACS.Common.Enums;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// Server status presenter
    /// </summary>
    public class ServerStatusPresenter : PresenterBase<IServerStatusView>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerStatusPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ServerStatusPresenter(IServerStatusView view)
            : base(view)
        {
        }

        /// <summary>
        /// Loads the status.
        /// </summary>
        public void LoadStatus()
        {
            try
            {
                DateTime? serverStartTime = GetStartTime(ApplicationSettings.Current.ServiceName);

                // get service status
                ServiceController sc = new ServiceController(ApplicationSettings.Current.ServiceName);

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        this.View.SetStatus(ServerStatus.Started, "Server is up and running.", serverStartTime, null);
                        break;
                    case ServiceControllerStatus.Stopped:
                        this.View.SetStatus(ServerStatus.Stopped, "Server has stopped.", null, null);
                        break;
                    case ServiceControllerStatus.Paused:
                        this.View.SetStatus(ServerStatus.Started, "Server is currently paused.", serverStartTime, null);
                        break;
                    case ServiceControllerStatus.StopPending:
                        this.View.SetStatus(ServerStatus.Started, "Server stop is pending.", serverStartTime, null);
                        break;
                    case ServiceControllerStatus.StartPending:
                        this.View.SetStatus(ServerStatus.Starting, "Server start is in progress.", null, null);
                        break;
                    default:
                        this.View.SetStatus(ServerStatus.Unknown, "Server state was changing. Please refresh.", null, null);
                        break;
                }

                // get the startup type
                this.View.SetStartupType(GetStartupType(ApplicationSettings.Current.ServiceName));
            }
            catch (Exception e)
            {
                this.View.SetStatus(ServerStatus.Error, "There was a problem loading the status of the server", null, e);
            }
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void StartServer()
        {
            try
            {
                ServiceController sc = new ServiceController(ApplicationSettings.Current.ServiceName);
                sc.Start();
                this.LoadStatus();
            }
            catch (InvalidOperationException ioe)
            {
                this.View.SetStatus(ServerStatus.Error, "There was a problem starting the server", null, ioe);
            }
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void StopServer()
        {
            try
            {
                ServiceController sc = new ServiceController(ApplicationSettings.Current.ServiceName);
                sc.Stop();
                this.LoadStatus();
            }
            catch (InvalidOperationException ioe)
            {
                this.View.SetStatus(ServerStatus.Error, "There was a problem stopping the server", null, ioe);
            }
        }

        /// <summary>
        /// Gets the start time for the process
        /// </summary>
        /// <param name="processName">Name of the process.</param>
        /// <returns></returns>
        internal static DateTime? GetStartTime(string processName)
        {
            Process[] processes = Process.GetProcessesByName("SACS.WindowsService");
            if (processes.Length == 0)
            {
                return null;
            }

            DateTime firstStartTime = DateTime.Now;
            foreach (Process p in processes)
            {
                if (p.StartTime < firstStartTime)
                {
                    firstStartTime = p.StartTime;
                }
            }

            return firstStartTime;
        }

        /// <summary>
        /// Gets the agent's startup type.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        internal static string GetStartupType(string serviceName)
        {
            RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\" + serviceName);

            int startupTypeValue = (int)reg.GetValue("Start");

            string startupType = string.Empty;

            switch (startupTypeValue)
            {
                case 0:
                    startupType = "BOOT";
                    break;
                case 1:
                    startupType = "SYSTEM";
                    break;
                case 2:
                    startupType = "AUTOMATIC";
                    break;
                case 3:
                    startupType = "MANUAL";
                    break;
                case 4:
                    startupType = "DISABLED";
                    break;
                default:
                    startupType = "UNKNOWN";
                    break;
            }

            return startupType;
        }
    }
}
