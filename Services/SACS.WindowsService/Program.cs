using System;
using System.Configuration;
using System.Diagnostics;
using Autofac;
using log4net;
using log4net.Config;
using SACS.BusinessLayer.BusinessLogic.Email;
using SACS.WindowsService.AppStart;
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

        private static ILog Log = LogManager.GetLogger(typeof(Program));
        private static ServiceState CurrentServiceState = ServiceState.Unknown;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the DI container
        /// </summary>
        /// <remarks>
        /// This should be private, but the OWIN setup needs work. Until then, there is no
        /// way of getting the container within the OWIN start up without it being set elsewhere.
        /// It is kept in <see cref="Program"/> since this is where it is used. Hopefully this
        /// can be changed back to private in a later release.
        /// </remarks>
        internal static IContainer Container { get; set; }

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
                Log.Error("Unhandled Exception occured in SACS service", ex);
                EmailHelper.SendSupportEmail(Container.Resolve<EmailProvider>(), ex, null);
            }
            catch (Exception finalEx)
            {
                try
                {
                    // At this point there is nothing more we can do but let the app die
                    Console.WriteLine("goodbye--^v--^v-----^v------------");
                    Log.Fatal("SACS has crashed.", finalEx);
                }
                finally
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Main start method
        /// </summary>
        /// <param name="args">The start method.</param>
        public static void Main(string[] args)
        {
            Initialize();
            var exitCode = StartWindowsService();

            CurrentServiceState = ServiceState.Unknown;
            switch (exitCode)
            {
                case TopshelfExitCode.Ok:
                    Log.Info("SACS stopped successfully.");
                    break;

                default:
                    Log.Error("Problem stopping SACS. " + exitCode.ToString());
                    break;
            }
        }

        /// <summary>
        /// Performs the container initialization.
        /// </summary>
        internal static void Initialize()
        {
            Log.Info("Starting SACS...");
            Container = DependencyConfig.RegisterComponents();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            BasicConfigurator.Configure();
            CurrentServiceState = ServiceState.Running;
        }

        /// <summary>
        /// Starts the windows service section. This must happen last as the HostFactory will block the thread.
        /// </summary>
        /// <returns></returns>
        private static TopshelfExitCode StartWindowsService()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                // this must be outside the HostFactory.Run since it risks failing silently
                var serviceContainer = Container.Resolve<ServiceContainer>();

                var exitCode = HostFactory.Run(x =>
                {
                    x.Service<ServiceContainer>(s =>
                    {
                        s.ConstructUsing(name => serviceContainer);
                        s.WhenStarted(sc => sc.Start());
                        s.WhenStopped(sc => sc.Stop());
                    });

                    x.SetDescription(ConfigurationManager.AppSettings[Constants.ServiceDescription]);
                    x.SetDisplayName(ConfigurationManager.AppSettings[Constants.ServiceDisplayName]);
                    x.SetServiceName(ConfigurationManager.AppSettings[Constants.ServiceName]);
                    x.UseLog4Net();

                    CurrentServiceState |= ServiceState.StartedWindowsComponent;
                });

                return exitCode;
            }
        }

        #endregion Methods
    }
}