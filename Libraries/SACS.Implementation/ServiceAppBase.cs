using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SACS.Implementation.Commands;
using SACS.Implementation.Execution;
using SACS.Implementation.Utils;

namespace SACS.Implementation
{
    /// <summary>
    /// Base class that service apps must implement to make use of SAC Server.
    /// </summary>
    [Serializable]
    public abstract class ServiceAppBase : MarshalByRefObject
    {
        #region Fields

        /// <summary>
        /// The interval (in milliseconds) between each check for execution.
        /// </summary>
        protected const int ExecutionInterval = 1000;

        /// <summary>
        /// The interval between when failed execution contexts (and other info) are cleaned up.
        /// </summary>
        private const int CleanUpInterval = 60;

        private static Mutex mutex = new Mutex();
        private static object _syncRoot = new object();
        private static object _syncExecution = new object();
        private static int cleanUpTimer = 0;
        private readonly CommandLineProcessor _commandProcessor;
        private readonly Timer _executionTimer;
        private readonly IList<ServiceAppContext> _executionContexts = new List<ServiceAppContext>();
        private int? _parentProcessId;
        private PipeStream pipeClientIn;
        private PipeStream pipeClientOut;
        private PipeStream pipeClientErr;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppBase" /> class.
        /// </summary>
        public ServiceAppBase()
        {
            FileLogger.Log("Created service app base");
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            // TODO: move this into it's own composition method.
            this._commandProcessor = new JsonCommandProcessor();
            this._commandProcessor.HoistWith<DirectiveHandler>("action")
                .For("run", () => this.QueueExecution())
                .For("stop", () => this.Stop())
                .For("hide", () => this.HideWindow());

            this._commandProcessor.HoistWith<DirectiveHandler>("info")
                .For("appVersion", () => this.EmitAppVersion())
                .For("sacsVersion", () => this.EmitSacsVersion());

            this._commandProcessor.HoistWith<DirectiveHandler>("owner")
                .ForArgs<int>(this.SetParentProcessId);

            this._commandProcessor.HoistWith<DirectiveHandler>("pipeIn")
                .ForArgs<string>(this.RedirectConsoleIn);

            this._commandProcessor.HoistWith<DirectiveHandler>("pipeOut")
                .ForArgs<string>(this.RedirectConsoleOut);

            this._commandProcessor.HoistWith<DirectiveHandler>("pipeErr")
                .ForArgs<string>(this.RedirectConsoleError);

            this._commandProcessor.HoistWith<DirectiveHandler>("parameters")
                .ForArgs<string>(this.SetParameters);

            this._commandProcessor.HoistWith<ArgsHandler>()
                .For("exit", () => this.Stop());

            this._executionTimer = new Timer(this.ExecutionTimer_Tick, null, 0, ExecutionInterval);
            cleanUpTimer = CleanUpInterval;
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets the version of this app.
        /// </summary>
        public string AppVersion
        {
            get
            {
                // Makes sense to change this also to the Version object instead of returning string.
                // However, for now the app version is determined elsewhere.
                return Assembly.GetAssembly(this.GetType()).GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the version of SACS.Implementation this is using.
        /// </summary>
        public Version SacsVersion
        {
            get
            {
                return Assembly.GetAssembly(typeof(ServiceAppBase)).GetName().Version;
            }
        }

        /// <summary>
        /// Gets the service app's execution mode when calling "run".
        /// </summary>
        public ExecutionMode ExecutionMode { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Service App is executing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App is executing; otherwise, <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get
            {
                return this._executionContexts.Any(c => c.IsExecuting && !c.Failed);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Service App is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the Service App has been stopped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App has been stopped; otherwise, <c>false</c>.
        /// </value>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// Gets the service app display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this.StartupCommands == null)
                {
                    throw new InvalidOperationException("Cannot get display name: ServiceApp has not been started yet");
                }

                string name;

                if (this.StartupCommands.GetCommands().ContainsKey("name"))
                {
                    name = this.StartupCommands.GetCommands()["name"] as string;
                }
                else
                {
                    name = Settings.AlternateName;
                }

                return name;
            }
        }

        /// <summary>
        /// Gets the custom service app parameters
        /// </summary>
        public string ServiceParameters
        {
            get
            {
                return Settings.Parameters;
            }
        }

        /// <summary>
        /// Gets the startup commands.
        /// </summary>
        protected CommandObject StartupCommands { get; private set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        protected void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.HandleException(e.ExceptionObject as Exception, true);
        }

        /// <summary>
        /// Handles the 'tick' event of the executionTimer.
        /// </summary>
        /// <param name="state">The state.</param>
        private void ExecutionTimer_Tick(object state)
        {
            if (this.IsStopped)
            {
                _executionTimer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }

            ServiceAppContext currentContext = null;

            switch (this.ExecutionMode)
            {
                case Execution.ExecutionMode.Default:
                case Execution.ExecutionMode.Idempotent:
                case Execution.ExecutionMode.Concurrent:
                    currentContext = this._executionContexts.FirstOrDefault(c => c.CanExecute);
                    break;

                case Execution.ExecutionMode.Inline:
                    currentContext = this._executionContexts.FirstOrDefault(c => c.CanExecute && !this.IsExecuting);
                    break;

                default:
                    throw new NotImplementedException("Execution mode not yet implemented");
            }

            if (currentContext != null)
            {
                Task executionTask = Task.Run(() =>
                {
                    Messages.WriteState(Enums.State.Executing);
                    Messages.WriteDebug("Starting exection for {0}. Time: {1}", this.DisplayName, currentContext.StartTime);
                    currentContext.StartTime = DateTimeResolver.Resolve();
                    Messages.WritePerformance(currentContext);
                    try
                    {
                        this.Execute(ref currentContext);
                        Messages.WriteState(Enums.State.Idle);
                    }
                    catch (Exception e)
                    {
                        currentContext.Failed = true;
                        currentContext.CustomMessage = e.Message;
                        this.HandleException(e, false);
                        Messages.WriteState(Enums.State.Failed);
                    }
                    finally
                    {
                        currentContext.EndTime = DateTimeResolver.Resolve();
                        Messages.WriteDebug("Ended exection for {0}. Duration: {1}", this.DisplayName, currentContext.Duration);
                        Messages.WritePerformance(currentContext);
                    }

                    this._executionContexts.Remove(currentContext);
                });

                currentContext.Handle = executionTask;
            }

            this.CleanUpServiceAppContexts();
            this.CheckParentProcessAlive();
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Executes this instance using the specified execution context.
        /// </summary>
        /// <param name="context">The current execution context. In derived classes that implement this method, this object
        /// contains information about the current execution.</param>
        /// <remarks>
        /// <para>
        /// The context is passed in by ref to prevent direct invocation of this outside of the SACS.Implemetation and
        /// passing in a null.
        /// </para>
        /// <para>
        /// ServiceAppContext is sealed and cannot be instantiated outside of the SACS.Implementation assembly. To run this
        /// method, use <see cref="QueueExecution"/>.
        /// </para></remarks>
        public abstract void Execute(ref ServiceAppContext context);

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>
        /// An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy
        /// for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a
        /// new lifetime service object initialized to the value of the
        /// <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This is overridden to prevent the service app from being garbage collected before it executes (typically if
        /// there is a delay of more than 4 minutes between executions). This happens, even though there is a reference
        /// to the object in the SAC, because this is marked as Marshall By Ref, which means that a proxy is used to
        /// communicate between app domains. The consequence being that the reference to the proxy might still be
        /// legitimate, but the referring object's lifetime is handled completely differently, and the garbage collector
        /// decides to take its liberties.
        /// </para>
        /// <para>
        /// UPDATE: Service apps are now plain executables. The override is still valid, however it now relates to the service
        /// apps instantiation during startup and keeping the object alive throughout the idle state of the exe while awaiting
        /// commands to execute.
        /// </para>
        /// <para>
        /// see https://msdn.microsoft.com/en-us/magazine/cc300474.aspx for more information on leases, sponsorship and
        /// object lifetime management.
        /// </para>
        /// </remarks>
        /// <PermissionSet>
        /// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Listens for lines from the standard input stream and processes them as commands.
        /// </summary>
        internal virtual void AwaitCommand()
        {
            while (this.IsLoaded)
            {
                string command = Console.ReadLine();
                this._commandProcessor.Process(command);
            }
        }

        /// <summary>
        /// Called when the service is being unloaded. this method should contain details on how to free up unmanaged resources.
        /// </summary>
        protected virtual void CleanUp()
        {
        }

        /// <summary>
        /// The main method to start the program as a service app.
        /// </summary>
        /// <param name="mode">The execution mode to start this service app as.</param>
        protected internal void Start(ExecutionMode mode = ExecutionMode.Default)
        {
            // TODO: rather use a mutex, but if this method gets called more than once, there a bigger problems
            lock (_syncRoot)
            {
                if (this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot start ServiceApp that has already stopped");
                }

                if (!this.IsLoaded)
                {
                    var startupArgs = Environment.CommandLine;
                    this.StartupCommands = this._commandProcessor.Parse(startupArgs);
                    this._commandProcessor.ProcessNonActions(this.StartupCommands);

                    FileLogger.Log("Startup args: " + startupArgs);
                    this.ExecutionMode = mode;

                    EmitSacsVersion();
                    Messages.WriteDebug("Starting {0}. Execution mode: {1}", this.DisplayName, this.ExecutionMode);
                    this.Initialize();
                    this.IsLoaded = true;
                    Messages.WriteState(Enums.State.Started);
                    this._commandProcessor.ProcessActions(this.StartupCommands);
                    this.AwaitCommand();

                    // We will get here after a "stop" command is processed as that will unblock the thread in
                    // AwaitCommand().
                    FinalizeServiceApp();
                }
                else
                {
                    throw new InvalidOperationException("ServiceApp has already started");
                }
            }
        }

        /// <summary>
        /// The main method to stop the program as a service app.
        /// </summary>
        internal void Stop()
        {
            if (this.IsLoaded)
            {
                FileLogger.Log("Stopping service app");
                Messages.WriteDebug("Stopping {0}. Execution mode: {1}", this.DisplayName, this.ExecutionMode);
                this.IsLoaded = false;
            }
        }

        /// <summary>
        /// Initializes this ServiceApp implementation. Place any once-off initializations in here.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Signals to start a new execution at the next available slot.
        /// </summary>
        protected void QueueExecution()
        {
            lock (_syncExecution)
            {
                if (!this.IsLoaded || this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot execute. Service app is not loaded or has stopped.");
                }

                bool createContext = false;

                switch (this.ExecutionMode)
                {
                    case ExecutionMode.Default:
                    case ExecutionMode.Idempotent:
                        if (!this._executionContexts.Any() || this._executionContexts.All(c => c.Failed))
                        {
                            createContext = true;
                        }

                        break;

                    case ExecutionMode.Inline:
                        createContext = true;
                        break;

                    case ExecutionMode.Concurrent:
                        if (Settings.MaxConcurrentExecutions == 0 || this._executionContexts.Count(c => c.IsExecuting) <= Settings.MaxConcurrentExecutions)
                        {
                            createContext = true;
                        }

                        break;

                    default:
                        throw new NotImplementedException("Execution mode not yet implemented");
                }

                if (createContext)
                {
                    var context = new ServiceAppContext()
                    {
                        Failed = false,
                        QueuedTime = DateTimeResolver.Resolve(),
                        Guid = Guid.NewGuid().ToString(),
                        Name = this.DisplayName
                    };

                    this._executionContexts.Add(context);
                }
            }
        }

        /// <summary>
        /// Aborts the process that is running the service app.
        /// </summary>
        private void AbortProcess()
        {
            try
            {
                FileLogger.Log("Force aborting service app");

                // Give the application a little bit of time to end gracefully afterwhich, commit suicide.
                mutex.WaitOne();
                Task t = Task.Run((Action)FinalizeServiceApp);
                t.Wait(ExecutionInterval * 5);
            }
            catch
            {
            }
            finally
            {
                mutex.ReleaseMutex();
                Process.GetCurrentProcess().Kill();
            }
        }

        /// <summary>
        /// Checks if the parent process is still alive.
        /// </summary>
        private void CheckParentProcessAlive()
        {
            if (!this._parentProcessId.HasValue)
            {
                // process was started stand-alone so nothing to check
                return;
            }

            bool abortProcess = false;

            try
            {
                var parentProcess = Process.GetProcessById(this._parentProcessId.Value);
                abortProcess = parentProcess.HasExited;
            }
            catch
            {
                // any issues accessing the state of the process means it's disappeared.
                abortProcess = true;
            }
            finally
            {
                if (abortProcess)
                {
                    AbortProcess();
                }
            }
        }

        /// <summary>
        /// Cleans up service application contexts.
        /// </summary>
        private void CleanUpServiceAppContexts()
        {
            cleanUpTimer--;

            if (cleanUpTimer <= 0)
            {
                for (int i = this._executionContexts.Count - 1; i >= 0; i--)
                {
                    var context = this._executionContexts[i];
                    if (context.Failed)
                    {
                        this._executionContexts.Remove(context);
                    }
                }

                cleanUpTimer = CleanUpInterval;
            }
        }

        /// <summary>
        /// The sends the app version to the output stream
        /// </summary>
        private void EmitAppVersion()
        {
            Messages.WriteResult(this.AppVersion);
        }

        /// <summary>
        /// The sends the SACS.Implementation version to the output stream
        /// </summary>
        private void EmitSacsVersion()
        {
            Messages.WriteVersion(this.SacsVersion);
        }

        /// <summary>
        /// Hides the console window.
        /// </summary>
        [Obsolete("This will be taken out in a future version")]
        private void HideWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The exception to handle.</param>
        /// <param name="clearContexts">If set to <c>true</c> [clear contexts].</param>
        private void HandleException(Exception e, bool clearContexts)
        {
            FileLogger.Log(e);
            Messages.WriteError(e);

            if (clearContexts)
            {
                this._executionContexts.Clear();
            }
        }

        private void RedirectConsoleIn(string pipeHandle)
        {
            FileLogger.Log("Redirecting Console In to handle: " + pipeHandle);
            this.pipeClientIn = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle);
            Console.SetIn(new StreamReader(this.pipeClientIn));
        }

        private void RedirectConsoleOut(string pipeHandle)
        {
            FileLogger.Log("Redirecting Console Out to handle: " + pipeHandle);
            this.pipeClientOut = new AnonymousPipeClientStream(PipeDirection.Out, pipeHandle);
            StreamWriter writer = new StreamWriter(this.pipeClientOut);
            writer.AutoFlush = true;
            Console.SetOut(writer);
        }

        private void RedirectConsoleError(string pipeHandle)
        {
            FileLogger.Log("Redirecting Console Error to handle: " + pipeHandle);
            this.pipeClientErr = new AnonymousPipeClientStream(PipeDirection.Out, pipeHandle);
            StreamWriter writer = new StreamWriter(this.pipeClientErr);
            writer.AutoFlush = true;
            Console.SetError(writer);
        }

        /// <summary>
        /// Sets the parameters in the Settings class.
        /// </summary>
        /// <param name="value">The parameters to set.</param>
        private void SetParameters(string value)
        {
            Settings.SetParameters(value);
        }

        /// <summary>
        /// Sets the parent process id.
        /// </summary>
        /// <param name="processId">The process id to set on the parent</param>
        /// <remarks>This is not a property because it is used in the command processor,
        /// which requires methods, not properties.</remarks>
        private void SetParentProcessId(int processId)
        {
            this._parentProcessId = processId;
        }

        /// <summary>
        /// Cleans up the service app so that it can close.
        /// </summary>
        private void FinalizeServiceApp()
        {
            if (!IsStopped)
            {
                FileLogger.Log("Cleaning up service app");
                CleanUp();
                Messages.WriteState(Enums.State.Stopped);
                IsStopped = true;
            }
        }

        #endregion Methods

        #region P/Invoke

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">The HDL.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        /// <summary>
        /// Gets the console window.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Show/hide the window.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="showCommand">Value represending the show command.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr windowHandle, int showCommand);

        #endregion P/Invoke
    }
}