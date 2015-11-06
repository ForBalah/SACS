using System;
using System.Configuration;
using System.Diagnostics;
using log4net;
using log4net.Config;
using SACS.BusinessLayer.BusinessLogic.Email;
using SACS.BusinessLayer.BusinessLogic.Loader;
using SACS.WindowsService.Common;
using SACS.WindowsService.Components;
using SACS.WindowsService.Enums;
using Topshelf;

namespace SACS.WindowsService
{
    /// <summary>
    /// Main entry point for the console version of the service
    /// </summary>
    public class Program
    {
        #region Fields

        private static ILog _log = LogManager.GetLogger(typeof(Program));
        private static ServiceState _serviceState = ServiceState.Unknown;
        private static EmailProvider _emailer = new EmailProvider();

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the UnhandledExepction event of the CurrentDomain object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The Unhandled Exception event args</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // gracefully handle the exception as best as possible.
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                _log.Error("Unhandled Exception occured in SACS service", ex);
                EmailHelper.SendSupportEmail(_emailer, ex, null);
            }
            catch
            {
                // At this point there is nothing more we can do but let the app die
                Console.WriteLine("goodbye--^v--^v-----^v------------");
                Process.GetCurrentProcess().Kill();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Main start method
        /// </summary>
        /// <param name="args">The start method.</param>
        public static void Main(string[] args)
        {
            Initialize();
            var exitCode = StartWindowsService();

            _serviceState = ServiceState.Unknown;
            switch (exitCode)
            {
                case TopshelfExitCode.Ok:
                    _log.Info("SACS stopped successfully.");
                    break;
                default:
                    _log.Info("Problem stopping SACS.");
                    break;
            }
        }

        /// <summary>
        /// Performs the container initialization.
        /// </summary>
        internal static void Initialize()
        {
            _log.Info("Starting SACS...");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            BasicConfigurator.Configure();
            _serviceState = ServiceState.Running;
        }

        /// <summary>
        /// Starts the windows service section. This must happen last as the HostFactory will block the thread.
        /// </summary>
        /// <returns></returns>
        private static TopshelfExitCode StartWindowsService()
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<ServiceContainer>(s =>
                {
                    s.ConstructUsing(name => new ServiceContainer(new ServiceAppSchedulingService(), new WebAPIComponent()));
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription(ConfigurationManager.AppSettings[Constants.ServiceDescription]);
                x.SetDisplayName(ConfigurationManager.AppSettings[Constants.ServiceDisplayName]);
                x.SetServiceName(ConfigurationManager.AppSettings[Constants.ServiceName]);
                x.UseLog4Net();

                _serviceState |= ServiceState.StartedWindowsComponent;
            });
            return exitCode;
        }

        #endregion
    }
}
