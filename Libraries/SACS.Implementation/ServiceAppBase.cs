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

        private readonly CommandLineProcessor _commandProcessor;
        private readonly object _syncRoot = new object();
        private readonly object _syncExecution = new object();
        private IList<ServiceAppContext> _executionContexts = new List<ServiceAppContext>();

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
            this._commandProcessor = new CommandLineProcessor();
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
        /// Handles the DomainUnload event of the ServiceAppBase object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void ServiceAppBase_DomainUnload(object sender, EventArgs e)
        {
            this.IsLoaded = false;
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
        /// Helper method for sending errors to a pre-defined logger. NOTE: stick to BCL (Base Class Library) exceptions
        /// as there could potentially be issues sending custom exceptions from a service app to its host SAC.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <remarks>
        /// <para>There are a lot of intricacies involved when raising events within a child domain that involves quite
        /// a bit of marshalling and serialization of assemblies between parent and child domains. Because the SAC and the
        /// Service App could sit in different locations, there can be quite a bit of time spent, by .NET, looking for
        /// the right assemblies. The codename for the assembly loader in .NET is "Fusion" and, if enabled, you can see all
        /// the locations that are checked for the right assembly to load.
        /// </para>
        /// <para>
        /// Because of the way in which the App Domains are setup to contain the service app, it proved to be difficult to
        /// indicate to Fusion where exactly to look for assemblies outside of the directory containing this service app
        /// implementation and the GAC.
        /// </para>
        /// <para>
        /// The easiest solution, without having to specify fairly complex "assembly resolving" for the binder, involves
        /// sticking to exception classes which are known to be common to both the service app container, and the service
        /// app. And all the derived exception classes within the BCL (E.g. ArgumentException or InvalidOperationException)
        /// meet the criteria.
        /// </para>
        /// <para>
        /// For an explanation around what communication steps take place between app domains, see:
        /// http://stackoverflow.com/questions/1277346/net-problem-with-raising-and-handling-events-using-appdomains.
        /// </para>
        /// </remarks>
        public void SendError(Exception exception)
        {
            if (exception != null && this.LogMessage != null)
            {
                this.LogMessage(this.GetType(), new MessageEventArgs { Message = new Message(this.GetType().Name, exception) });
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
        /// The context is passed in by ref to prevent running this outside of the SACS.Implemetation and passing in a null.
        /// </para>
        /// <para>
        /// ServiceAppContext is sealed and cannot be instantiated outside of the SACS.Implementation assembly. To run this
        /// method, use Run().
        /// </para></remarks>
        public abstract void Execute(ref ServiceAppContext context);

        /// <summary>
        /// Performs the execution of this service app instance
        /// </summary>
        protected void Run()
        {
            lock (this._syncRoot)
            {
                if (!this.IsLoaded || this.IsStopped)
                {
                    throw new InvalidOperationException("Cannot execute ServiceApp is not loaded or has stopped.");
                }

                lock (this._syncExecution)
                {
                    bool createContext = true;
                    bool executeImmediately = false;

                    switch (this.ExecutionMode)
                    {
                        case ExecutionMode.Default:
                        case ExecutionMode.Idempotent:
                            if (this._executionContexts.Any(c => c.IsExecuting))
                            {
                                createContext = false;
                            }
                            else
                            {
                                executeImmediately = true;
                            }
                            break;

                        case ExecutionMode.Inline:
                            executeImmediately = !this._executionContexts.Any(c => c.IsExecuting);
                            break;

                        case ExecutionMode.Unbounded:
                            executeImmediately = true;
                            break;

                        default:
                            throw new NotImplementedException("Execution mode not implemented");
                    }

                    if (createContext)
                    {
                        // TODO: refactor
                        var context = new ServiceAppContext()
                        {
                            Failed = false,
                            IsExecuting = executeImmediately,
                            QueuedTime = DateTimeResolver.Resolve(),
                            ContextId = Thread.CurrentThread.ManagedThreadId
                        };

                        this._executionContexts.Add(context);
                    }
                }
            }
        }

        /// <summary>
        /// The main method to start the program as a service app
        /// </summary>
        /// <param name="mode">The execution mode to start this service app as.</param>
        protected void Start(ExecutionMode mode)
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
                    var startupArgs = Environment.GetCommandLineArgs();
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

        #endregion Methods
    }
}