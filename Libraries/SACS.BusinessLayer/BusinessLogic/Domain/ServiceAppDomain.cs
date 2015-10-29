using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Loader;
using SACS.BusinessLayer.BusinessLogic.Security;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;
using SACS.Implementation;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// ServiceApp domain business entity wrapper
    /// </summary>
    [Serializable]
    [Obsolete("Dropped in favour or ServiceAppProcess")]
    public class ServiceAppDomain : MarshalByRefObject
    {
        #region Fields

        private readonly IServiceAppDao _executionDao = DaoFactory.Create<IServiceAppDao, ServiceAppDao>();
        private readonly DomainInitializer initializer = new DomainInitializer();
        private Timer _unloadCheckTimer;
        private ServiceApp _ServiceApp;
        private ServiceAppBase _serviceImpl;
        private AppDomain _appDomain;
        private ILog _log;
        private ServiceAppImpersonator _impersonator;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppDomain" /> class.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="log">The log.</param>
        /// <param name="impersonator">The user impersonator provider.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Cannot create ServiceAppDomain with null ServiceApp
        /// or
        /// Cannot create ServiceAppDomain with null or empty ServiceApp name
        /// </exception>
        internal ServiceAppDomain(ServiceApp app, ILog log, ServiceAppImpersonator impersonator)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app", "Cannot create ServiceAppDomain with null ServiceApp");
            }

            if (string.IsNullOrWhiteSpace(app.Name))
            {
                throw new ArgumentNullException("app", "Cannot create ServiceAppDomain with null or empty ServiceApp name");
            }

            this.Messages = new List<Tuple<string, ServiceAppState>>();
            this._ServiceApp = app;
            this._log = log;
            this._impersonator = impersonator;
            this._unloadCheckTimer = new Timer(1000);
            this._unloadCheckTimer.Elapsed += this.UnloadCheckTimer_Elapsed;
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets the actual AppDomain associated with this ServiceAppDomain.
        /// </summary>
        public AppDomain AppDomain
        {
            get
            {
                return this._appDomain;
            }
        }

        /// <summary>
        /// Gets the service app.
        /// </summary>
        /// <value>
        /// The service app.
        /// </value>
        public ServiceApp ServiceApp
        {
            get
            {
                // TODO: only make this property return the service app. These are side-effects and that's bad.
                this._ServiceApp.StateEnum = this.CurrentState;
                this._ServiceApp.LastMessage = this.Messages.Select(m => m.Item1).LastOrDefault();
                return this._ServiceApp;
            }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public ServiceAppState CurrentState
        {
            get
            {
                if (this.Messages.Any())
                {
                    return this.Messages.LastOrDefault().Item2;
                }

                return ServiceAppState.Unknown;
            }
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IList<Tuple<string, ServiceAppState>> Messages
        {
            get;
            private set;
        }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the Elapsed event of the UnloadCheckTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void UnloadCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this._appDomain.FriendlyName))
                {
                    // HACK: accessing the name should cause an exception showing that the domain is no longer loaded.
                }
            }
            catch (System.AppDomainUnloadedException)
            {
                this._unloadCheckTimer.Stop();
                this._appDomain = null;
                this.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.SuccessfulUnload, ServiceAppState.NotLoaded));
                this._log.Info("Succesfully unloaded " + this.ServiceApp.Name);
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Gets the execution job.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Thrown when the service app was not initialized successfully to be able to schedule the job.</exception>
        internal void RunServiceApp()
        {
            var job = new ServiceAppJobWrapper(this._serviceImpl, this._executionDao, this._impersonator, appName: this.ServiceApp.Name, username: this.ServiceApp.Username, password: this.ServiceApp.Password);
            job.Execute();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal virtual void Initialize()
        {
            switch (this.CurrentState)
            {
                case ServiceAppState.Initialized:
                    throw new InvalidOperationException("The ServiceAppDomain is already initialized");
                case ServiceAppState.Unloading:
                    throw new InvalidOperationException("The ServiceAppDomain has already been marked to unload. Create a new ServiceAppDomain.");
            }

            try
            {
                this.CreateAppDomain();
                string entryType = this.initializer.GetEntryType(this.ServiceApp.AssemblyPath);

                this._serviceImpl = (ServiceAppBase)this._appDomain.CreateInstanceAndUnwrap(this.ServiceApp.Assembly, entryType);
                //this._impersonator.RunAsUser(this.ServiceApp.Username, this.ServiceApp.Password, () => this._serviceImpl.DomainInitialize());
                //this._serviceImpl.IsLoaded = true;
                //this._appDomain.DomainUnload += this._serviceImpl.ServiceAppBase_DomainUnload;

                this.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.SuccesfulInitialization, ServiceAppState.Initialized));
                this._log.Info(string.Format("{0} initialized successfully. Domain name is: {1}", this.ServiceApp.Name, this._appDomain.FriendlyName));
            }
            catch (Exception e)
            {
                this._log.Error(string.Format("{0} ServiceApp could not be initialized.", this.ServiceApp.Name, this._appDomain.FriendlyName), e);
                if (e is EntryPointNotFoundException || e is Win32Exception)
                {
                    this.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.InitializedWithError, ServiceAppState.Error));
                    this.Unload(); // put it into a stable state
                }
                else
                {
                    this.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.FailedToInitialize, ServiceAppState.Error));
                    this._appDomain = null; // nothing loaded properly. Reset everything.
                }
            }
        }

        /// <summary>
        /// Unloads the application domain.
        /// </summary>
        internal void Unload()
        {
            if (this._appDomain != null && this.CurrentState != ServiceAppState.Unloading)
            {
                try
                {
                    this._log.Info("Unloading ServiceApp " + this.ServiceApp.Name);
                    //this._impersonator.RunAsUser(this.ServiceApp.Username, this.ServiceApp.Password, () => this._serviceImpl.DomainCleanUp());
                }
                catch (Exception e)
                {
                    this._log.Warn(string.Format("ServiceApp {0} errored out when destroying", this.ServiceApp.Name), e);
                }
                finally
                {
                    this.Messages.Add(new Tuple<string, ServiceAppState>(ServiceAppMessages.Unloading, ServiceAppState.Unloading));
                    AppDomain.Unload(this._appDomain);
                    this._unloadCheckTimer.Start(); // we use a timer because we can't bind directly to the app domain unload event (serialization issues)
                }
            }
        }

        /// <summary>
        /// Creates the app domain.
        /// </summary>
        private void CreateAppDomain()
        {
            AppDomainSetup ads = new AppDomainSetup()
            {
                ApplicationBase = this.ServiceApp.Path,
                DisallowBindingRedirects = false, // important especially if an app has its .NET version upgraded
                DisallowCodeDownload = true, // prevents services from downloading partially trusted code, and keeps our server safe
                ConfigurationFile = this.ServiceApp.ConfigFilePath,
                ////PrivateBinPath = Path.GetDirectoryName(this.GetType().Assembly.Location)
            };

            this._appDomain = AppDomain.CreateDomain(string.Format("ServiceApp.{0}", this.ServiceApp.Name), null, ads);
        }

        /// <summary>
        /// Logs the unhandled exception.
        /// </summary>
        /// <param name="message">The message.</param>
        private void LogUnhandledException(Message message)
        {
            this._log.Error(message.Value);
        }

        #endregion Methods
    }
}