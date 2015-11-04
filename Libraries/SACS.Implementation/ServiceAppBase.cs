using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SACS.Implementation.Commands;
using SACS.Implementation.Execution;
using SACS.Implementation.Utils;

namespace SACS.Implementation
{
    /// <summary>
    /// Base class that service apps must implement to make use of SAC Server.
    /// For ease-of-use, the class implementing this must expose a parameter-less
    /// constructor.
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
        /// The interval between when failed execution contexts (and other info) are cleaned up
        /// </summary>
        private const int CleanUpInterval = 60;
        private readonly CommandLineProcessor _commandProcessor;
        private readonly Timer _executionTimer;
        private readonly IList<ServiceAppContext> _executionContexts = new List<ServiceAppContext>();
        private static object _syncRoot = new object();
        private static object _syncExecution = new object();
        private static int cleanUpTimer = 0;
        private int _parentProcessId = 0;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppBase" /> class.
        /// </summary>
        public ServiceAppBase()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // TODO: move this into it's own composition method.
            this._commandProcessor = new JsonCommandProcessor();
            this._commandProcessor.HoistWith<ActionProcessor>()
                .For("run", () => this.QueueExecution())
                .For("stop", () => this.Stop());

            this._commandProcessor.HoistWith<ArgsProcessor>()
                .For("exit", () => this.Stop());

            this._executionTimer = new Timer(ExecutionTimer_Tick, null, 0, ExecutionInterval);
            cleanUpTimer = CleanUpInterval;
        }

        #endregion Constructors and Destructors

        #region Properties

        /// <summary>
        /// Gets or sets the service app's execution mode when calling run.
        /// </summary>
        public ExecutionMode ExecutionMode { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Service App is executing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App is executing; otherwise, <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get
            {
                return _executionContexts.Any(c => c.IsExecuting && !c.Failed);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Service App is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Service App has been stopped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Service App has been stopped; otherwise, <c>false</c>.
        /// </value>
        public bool IsStopped { get; private set; }

        /// <summary>
        /// Gets the service app display name
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

                if (this.StartupCommands.ContainsKey("name"))
                {
                    name = string.Format("{0} ({1})", this.StartupCommands["name"] as string, Settings.AlternateName);
                }
                else
                {
                    name = Settings.AlternateName;
                }

                return name;
            }
        }

        /// <summary>
        /// Gets the startup commands
        /// </summary>
        protected IDictionary<string, object> StartupCommands { get; private set; }

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
        /// Handles the 'tick' event of the executionTimer
        /// </summary>
        /// <param name="state">The state.</param>
        private void ExecutionTimer_Tick(object state)
        {
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
                    Messages.WritePerformance(currentContext, null);
                    try
                    {
                        this.Execute(ref currentContext);
                        Messages.WriteState(Enums.State.Idle);
                    }
                    catch (Exception e)
                    {
                        currentContext.Failed = true;
                        this.HandleException(e, false);
                        Messages.WriteState(Enums.State.Failed);
                    }
                    finally
                    {
                        currentContext.EndTime = DateTimeResolver.Resolve();
                        Messages.WriteDebug("Ended exection for {0}. Duration: {1}", this.DisplayName, currentContext.Duration);
                        Messages.WritePerformance(currentContext, null);
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
        /// information about the current execution.</param>
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
        /// The main method to start the program as a service app
        /// </summary>
        /// <param name="mode">The execution mode to start this service app as.</param>
        protected internal void Start(ExecutionMode mode = Execution.ExecutionMode.Default)
        {
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
                    this.ExecutionMode = mode;

                    Messages.WriteDebug("Starting {0}. Execution mode: {1}", this.DisplayName, this.ExecutionMode);
                    this.Initialze();
                    this.IsLoaded = true;
                    Messages.WriteState(Enums.State.Started);
                    this.AwaitCommand();
                }
            }
        }

        /// <summary>
        /// The main method to stop the program as a service app
        /// </summary>
        internal void Stop()
        {
            Messages.WriteDebug("Stopping {0}. Execution mode: {1}", this.DisplayName, this.ExecutionMode);
            this._executionTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (this.IsLoaded)
            {
                this.IsLoaded = false;
                this.CleanUp();
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    Thread.Sleep(1000);
                    IntPtr stdin = GetStdHandle(StdHandle.Stdin);
                    CloseHandle(stdin);
                    this.IsStopped = true;
                    Messages.WriteState(Enums.State.Stopped);
                });
            }
        }

        /// <summary>
        /// Initializes this ServiceApp implementation. Place any once-off initializations in here.
        /// </summary>
        protected abstract void Initialze();

        /// <summary>
        /// Signals to start a new execution at the next available slot.
        /// </summary>
        protected void QueueExecution()
        {
            lock (_syncExecution)
            {
                if (!this.IsLoaded || this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot execute ServiceApp is not loaded or has stopped.");
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
        /// Checks if the parent process is still alive.
        /// </summary>
        private void CheckParentProcessAlive()
        {
            var parent = Process.GetCurrentProcess().Parent();
            bool canStop = false;

            if (parent != null)
            {
                this._parentProcessId = parent.Id;
                if (parent.HasExited)
                {
                    canStop = true;
                }
            }
            else if (this._parentProcessId != 0)
            {
                // this means that there was a parent process, but it nolonger exists. exit the app
                canStop = true;
            }

            if (canStop)
            {
                this.Stop();
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
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The exception to handle.</param>
        /// <param name="clearContexts">if set to <c>true</c> [clear contexts].</param>
        private void HandleException(Exception e, bool clearContexts)
        {
            string message = "Unhandled exception in {0}";
            Messages.WriteError(e, message, this.DisplayName);

            if (clearContexts)
            {
                this._executionContexts.Clear();
            }
        }

        #endregion Methods

        #region P/Invoke

        // P/Invoke:
        private enum StdHandle { Stdin = -10, Stdout = -11, Stderr = -12 };

        /// <summary>
        /// Gets the standard handle.
        /// </summary>
        /// <param name="std">The standard.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(StdHandle std);

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="hdl">The HDL.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hdl);

        #endregion
    }
}