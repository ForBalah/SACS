using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly CommandLineProcessor _commandProcessor;
        private readonly object _syncRoot = new object();
        private readonly object _syncExecution = new object();
        private readonly Timer _executionTimer;
        private readonly IList<ServiceAppContext> _executionContexts = new List<ServiceAppContext>();

        #endregion Fields

        #region Events

        [Obsolete("dropped in favour of writing to the standard output")]
        public event EventHandler<MessageEventArgs> LogMessage;

        #endregion Events

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppBase" /> class.
        /// </summary>
        public ServiceAppBase()
        {
            this._executionTimer = new Timer(ExecutionTimer_Tick, null, 0, ExecutionInterval);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this._commandProcessor = new JsonCommandProcessor();
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
                return _executionContexts.Any(c => c.IsExecuting);
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

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        protected void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // todo: send error message
        }

        /// <summary>
        /// Handles the 'tick' event of the executionTimer
        /// </summary>
        /// <param name="state">The state.</param>
        private void ExecutionTimer_Tick(object state)
        {
            ServiceAppContext currentContext = this._executionContexts.FirstOrDefault(c => !c.Failed);

            if (currentContext != null)
            {
                currentContext.IsExecuting = true;
                currentContext.StartTime = DateTimeResolver.Resolve();
                this.Execute(ref currentContext);
                currentContext.EndTime = DateTimeResolver.Resolve();
                currentContext.IsExecuting = false;
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Helper method for sending info messages to a pre-defined logger.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessage(string message)
        {
            if (message != null && this.LogMessage != null)
            {
                this.LogMessage(null, new MessageEventArgs { Message = new Message(this.GetType().Name, message) });
            }
        }

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
        /// The main method to start the program as a service app
        /// </summary>
        /// <param name="mode">The execution mode to start this service app as.</param>
        protected void Start(ExecutionMode mode = Execution.ExecutionMode.Default)
        {
            lock (this._syncRoot)
            {
                if (this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot start ServiceApp that has already stopped");
                }

                if (!this.IsLoaded)
                {
                    this.ExecutionMode = mode;
                    this.Initialze();
                    this.IsLoaded = true;
                    var startupArgs = Environment.CommandLine;
                    this._commandProcessor.Process(startupArgs);
                    this.AwaitCommand();
                }
            }
        }

        /// <summary>
        /// The main method to stop the program as a service app
        /// </summary>
        protected void Stop()
        {
            lock (this._syncRoot)
            {
                this.IsLoaded = false;
                this.CleanUp();
                this.IsStopped = true;
            }
        }

        /// <summary>
        /// Called when the service is being unloaded. this method should contain details on how to free up unmanaged resources.
        /// </summary>
        protected virtual void CleanUp()
        {
        }

        /// <summary>
        /// Initializes this ServiceApp implementation. Place any once-off initializations in here.
        /// </summary>
        protected abstract void Initialze();

        /// <summary>
        /// listens for lines from the standard input stream and processes them as commands.
        /// </summary>
        private void AwaitCommand()
        {
            while (this.IsLoaded)
            {
                string command = Console.ReadLine();
                this._commandProcessor.Process(command);
            }
        }

        /// <summary>
        /// Signals to start a new execution at the next available slot.
        /// </summary>
        protected void QueueExecution()
        {
            lock (this._syncExecution)
            {
                if (!this.IsLoaded || this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot execute ServiceApp is not loaded or has stopped.");
                }

                bool createContext = true;

                switch (this.ExecutionMode)
                {
                    case ExecutionMode.Default:
                    case ExecutionMode.Idempotent:
                        if (this._executionContexts.Any(c => c.IsExecuting))
                        {
                            createContext = false;
                        }
                        break;

                    case ExecutionMode.Inline:
                    case ExecutionMode.Concurrent:
                    default:
                        throw new NotImplementedException("Execution mode not yet implemented");
                }

                if (createContext)
                {
                    var context = new ServiceAppContext()
                    {
                        Failed = false,
                        QueuedTime = DateTimeResolver.Resolve(),
                        Guid = Guid.NewGuid().ToString()
                    };

                    this._executionContexts.Add(context);
                }
            }
        }

        #endregion Methods
    }
}