using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using SACS.BusinessLayer.BusinessLogic.Errors;
using SACS.BusinessLayer.Extensions;
using SACS.Common.Configuration;
using SACS.Common.Enums;
using SACS.Common.Factories.Interfaces;
using SACS.Common.Helpers;
using SACS.Common.Runtime;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// The service app wrapper for runtime use.
    /// </summary>
    [Serializable]
    public class ServiceAppProcess : MarshalByRefObject
    {
        #region Fields

        private ProcessWrapper _process;
        private ILog _log;
        private Task _runTask;
        private List<Tuple<string, ServiceAppState>> _Messages;
        private IProcessWrapperFactory _processFactory;
        private Action _MonitorProcessCallback;
        private const string WarningMessage = "{0} performance warning: Process could not be found for service app {1}. Check that it started correctly.";
        private const int ProcessWaitLimit = 10000;
        private bool _canRecreateProcess = true;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ServiceAppProcess"/> class from being created.
        /// </summary>
        private ServiceAppProcess()
        {
            // For use by tests. I'm sure there's a better way, but at the time I didn't
            // want to be concerned with the workings of the process wrapper in the tests that relied
            // on this (and was too lazy to create an abstraction and change usage all over)
            this.ServiceApp = new ServiceApp { Name = "__Test" };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppProcess"/> class.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="log">The log.</param>
        /// <param name="processFactory">The factory that creates <see cref="ProcessWrapper"/> instances.</param>
        /// <exception cref="System.ArgumentNullException">app;Cannot create ServiceAppProcess with null ServiceApp</exception>
        /// <exception cref="System.ArgumentException">Cannot create ServiceAppProcess with null or empty ServiceApp name</exception>
        internal ServiceAppProcess(ServiceApp app, ILog log, IProcessWrapperFactory processFactory)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app", "Cannot create ServiceAppProcess with null ServiceApp");
            }

            if (string.IsNullOrWhiteSpace(app.Name))
            {
                throw new ArgumentException("Cannot create ServiceAppProcess with null or empty ServiceApp name");
            }

            ServiceApp = app;
            _log = log;
            _processFactory = processFactory;
            _Messages = new List<Tuple<string, ServiceAppState>>();

            CreateProcess();
        }

        #endregion Constructors and Destructors

        #region Events

        /// <summary>
        /// Occurs when the service app has successfully started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the service app is executing.
        /// </summary>
        public event EventHandler Executing;

        /// <summary>
        /// Occurs when the service app has successfully executed.
        /// </summary>
        public event EventHandler<ServiceAppSuccessEventArgs> Executed;

        /// <summary>
        /// Occurs when the service app generated an uncaught error.
        /// </summary>
        public event EventHandler<ServiceAppErrorEventArgs> Error;

        /// <summary>
        /// Occurs when the service app has successfully stopped.
        /// </summary>
        public event EventHandler<ServiceAppStopEventArgs> Stopped;

        /// <summary>
        /// Occurs when the service app failed.
        /// </summary>
        public event EventHandler Failed;

        /// <summary>
        /// Occurs when the service app reports performance information.
        /// </summary>
        public event EventHandler<ServiceAppPerformanceEventArgs> Performance;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the entropy value.
        /// </summary>
        /// <remarks>This is kept here and not on the ServiceApp to make sure that if any information
        /// other than password is changed, then the entropy won't be altered.</remarks>
        public string EntropyValue
        {
            get; set;
        }

        /// <summary>
        /// Gets a value indicating whether the process is running, but has an error.
        /// </summary>
        public bool HasError
        {
            get
            {
                return this.ServiceApp.CurrentState == ServiceAppState.Error;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying process is running.
        /// </summary>
        /// <returns></returns>
        public bool IsProcessRunning
        {
            get
            {
                if (this._process == null)
                {
                    return false;
                }

                try
                {
                    return this._process.StartTime > default(DateTime) && !this._process.HasExited;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IReadOnlyList<Tuple<string, ServiceAppState>> Messages
        {
            get
            {
                return this._Messages;
            }
        }

        /// <summary>
        /// Gets or sets the service app.
        /// </summary>
        /// <value>
        /// The service app.
        /// </value>
        public virtual ServiceApp ServiceApp
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the callback that is invoked after the underlying process is started.
        /// This usually involves monitoring said process.
        /// </summary>
        internal Action MonitorProcessCallback
        {
            get
            {
                if (_MonitorProcessCallback != null)
                {
                    return _MonitorProcessCallback;
                }

                return MonitorProcess;
            }

            set
            {
                _MonitorProcessCallback = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a message to this process and sets the state on this (and the underlying ServiceApp) accordingly.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <param name="state">The state associated with the message.</param>
        public void AddMessage(string message, ServiceAppState state)
        {
            this._Messages.Add(new Tuple<string, ServiceAppState>(message, state));
            this.ServiceApp.CurrentState = state;
            this.ServiceApp.LastMessage = message;
        }

        /// <summary>
        /// Executes the service app.
        /// </summary>
        public virtual void ExecuteServiceApp()
        {
            this._process.StandardInput.WriteLine(JsonConvert.SerializeObject(new { action = "run" }));
        }

        /// <summary>
        /// Starts the associated service app.
        /// </summary>
        public virtual Task Start()
        {
            _runTask = Task.Run((Action)StartProcess);
            return _runTask;
        }

        /// <summary>
        /// Stops the associated service app.
        /// </summary>
        /// <param name="isApplicationExiting">Set to true to prevent the service app from recovering from a dirty 
        /// state on stop since the application will be exiting.</param>
        public virtual void Stop(bool isApplicationExiting)
        {
            _canRecreateProcess = !isApplicationExiting;

            try
            {
                _process.StandardInput.WriteLine(JsonConvert.SerializeObject(new { action = "stop" }));

                // we will need to be doubly sure that the process has ended.
                // ---
                // Would have preferred Process.WaitForExit but we also need to synchronize with the other thread 
                // to process the "I have stopped" state change from the process. There probably is a better way 
                // of doing this...
                int attempts = 5;
                while (attempts > 0)
                {
                    if (IsProcessRunning || ServiceApp.CurrentState != ServiceAppState.NotLoaded)
                    {
                        Thread.Sleep(ProcessWaitLimit / attempts);
                        attempts--;
                    }
                    else
                    {
                        break;
                    }
                }

                if (attempts <= 0)
                {
                    try
                    {
                        _process.Kill();
                    }
                    finally
                    {
                        AddMessage("Service app force stopped.", ServiceAppState.NotLoaded);
                    }
                }

                // stop listening on the client out stream
                _process.StandardOutput.Close();
            }
            catch (InvalidOperationException ioe)
            {
                // the process must have died already but the state of this probably did not update correctly
                _log.Debug(ioe);
            }
            catch (IOException ioex)
            {
                // this means that the "pipes are broken"
                _log.Debug(ioex);
            }

            if (_canRecreateProcess)
            {
                if (!IsProcessRunning)
                {
                    if (ServiceApp.CurrentState != ServiceAppState.NotLoaded)
                    {
                        // getting to this point usually means that the app was stopped before the "stop"
                        // state was emitted and processed.
                        AddMessage("Process exited unexpectedly.", ServiceAppState.NotLoaded);
                    }

                    // This is done in here to ensure that the object is never left in a dirty state.
                    // This is fine because the process is not running yet and just exists as a .NET object
                    // so as to prevent null reference exceptions.
                    CreateProcess();
                }
                else
                {
                    throw new ApplicationException("Cannot re-create the service app's process as it failed to exit.");
                }
            }
        }

        /// <summary>
        /// Gets the current Memory performance
        /// </summary>
        /// <returns></returns>
        internal virtual decimal GetCurrentRamValue()
        {
            try
            {
                PerformanceCounter ramCounter = new PerformanceCounter("Process", "Working Set - Private", this.GetProcessInstanceName());
                decimal value = (decimal)ramCounter.NextValue();
                return value;
            }
            catch (InvalidOperationException)
            {
                // the process is not running.
                return 0;
            }
            catch (KeyNotFoundException)
            {
                // process could not be found - it might have been killed.
                _log.Error(string.Format("Process for service app {0} has been removed.", ServiceApp.Name));
                AbortProcess();
                return 0;
            }
        }

        /// <summary>
        /// Gets the current CPU performance
        /// </summary>
        /// <returns></returns>
        internal virtual decimal GetCurrentCpuValue()
        {
            try
            {
                PerformanceCounter cpuCounter = new PerformanceCounter("Process", "% Processor Time", this.GetProcessInstanceName());
                decimal value = (decimal)cpuCounter.NextValue();
                return value;
            }
            catch (InvalidOperationException)
            {
                // the process is not running.
                return 0;
            }
            catch (KeyNotFoundException)
            {
                // process could not be found - it might have been killed.
                _log.Error(string.Format("Process for service app {0} has been removed.", ServiceApp.Name));
                AbortProcess();
                return 0;
            }
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if the process should exit after processing the message.</returns>
        /// <exception cref="System.NotImplementedException">Nothing was able to process the provided message.</exception>
        internal virtual bool ProcessMessage(string message)
        {
            bool stopProcessingMessages = false;
            try
            {
                dynamic messageObject = JsonConvert.DeserializeObject(message);
                if (messageObject != null)
                {
                    if (messageObject.debug != null)
                    {
                        ProcessDebug(messageObject.debug.Value as string);
                    }
                    else if (messageObject.info != null)
                    {
                        ProcessInfo(messageObject.info.Value as string);
                    }
                    else if (messageObject.error != null)
                    {
                        ProcessError(
                            ExceptionHelper.DeserializeException(messageObject.error.exception.Value as string),
                            messageObject.error.details.type.Value as string,
                            messageObject.error.details.message.Value as string,
                            messageObject.error.details.source.Value as string,
                            messageObject.error.details.stackTrace.Value as string);
                    }
                    else if (messageObject.performance != null)
                    {
                        this.ProcessPerformance(messageObject.performance);
                    }
                    else if (messageObject.state != null)
                    {
                        stopProcessingMessages = ProcessState(messageObject.state.Value as string);
                    }
                    else if (messageObject.result != null)
                    {
                        ProcessResult(messageObject.result.Value as string);
                    }
                }
                else
                {
                    throw new NotImplementedException("Unrecognized message to process.");
                }
            }
            catch (JsonReaderException)
            {
                // this must be just a normal info message. process it as such
                ProcessInfo(message);
            }
            catch (ArgumentNullException)
            {
                // A null message means that communication with the process has been severed.
                // Opt to stop as it means the process might have been killed.
                stopProcessingMessages = AbortProcess();
            }

            return stopProcessingMessages;
        }

        /// <summary>
        /// Gets the true instance name for the process which is running this service app.
        /// </summary>
        /// <returns></returns>
        internal virtual string GetProcessInstanceName()
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {
                using (PerformanceCounter cnt = new PerformanceCounter("Process", "ID Process", instance, true))
                {
                    int val = (int)cnt.RawValue;
                    if (val == this._process.Id)
                    {
                        return instance;
                    }
                }
            }

            throw new KeyNotFoundException("Could not find performance counter instance name for current process. This is truly strange ...");
        }

        /// <summary>
        /// Processes the performance object
        /// </summary>
        /// <param name="performanceObject">The performace object to process.</param>
        private void ProcessPerformance(dynamic performanceObject)
        {
            if (this.Performance != null)
            {
                ServiceAppPerformanceEventArgs args = new ServiceAppPerformanceEventArgs(
                    name: performanceObject.name.Value as string,
                    guid: performanceObject.guid.Value as string,
                    failed: (bool)performanceObject.failed.Value,
                    startTime: performanceObject.startTime.Value as DateTime?,
                    endTime: performanceObject.endTime.Value as DateTime?,
                    message: performanceObject.message.Value as string);

                // TODO: should this be here?
                ServiceApp.LastRun = performanceObject.startTime.Value as DateTime?;

                this.Performance(this, args);
            }
        }

        /// <summary>
        /// Processes the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <param name="stackTrace">The stacktrace.</param>
        /// <remarks>
        /// This may seem like a weird way of dealing with the exception, however
        /// at the time there was no known way of deserializing the exception correctly
        /// without going down the route of marshalling the exception and encoding it in
        /// base64 etc... That is still a viable option since this current method feels
        /// quite dirty.
        /// </remarks>
        private void ProcessError(Exception exception, string type, string message, string source, string stackTrace)
        {
            Exception finalException = exception ?? new CustomException(string.Format("Type:{0} Message:\"{0}\" Source:{1}", type, message, source), stackTrace);

            // Somehow we are not getting the stack trace from the exception, so it had to be printed manually
            _log.Warn(string.Format("Uncaught error in {0}{1}{1}{2}{1}{3}{1}{4}", ServiceApp.Name, Environment.NewLine, finalException.GetType(), finalException.Message, finalException.StackTrace));

            if (Error != null)
            {
                Error(this, new ServiceAppErrorEventArgs(finalException, ServiceApp.Name));
            }
        }

        /// <summary>
        /// Processes the debug message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void ProcessDebug(string message)
        {
            _log.Debug(string.Format("From {0}: {1}", ServiceApp.Name, message));
        }

        /// <summary>
        /// Processes the info message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void ProcessInfo(string message)
        {
            _log.Info(message);
        }

        /// <summary>
        /// Processes the result value from the service app, optionally sending the results if configured to do so.
        /// </summary>
        /// <param name="result">The result message to process.</param>
        private void ProcessResult(string result)
        {
        }

        /// <summary>
        /// Processes the state.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        private bool ProcessState(string stateName)
        {
            bool forceExit = false;
            switch (stateName)
            {
                case "Started":
                    this.AddMessage(string.Format("{0} has started", this.ServiceApp.Name), ServiceAppState.Ready);

                    if (this.Started != null)
                    {
                        this.Started(this, new EventArgs());
                    }

                    break;

                case "Executing":
                    this.AddMessage(string.Format("{0} is executing", this.ServiceApp.Name), ServiceAppState.Executing);

                    if (this.Executing != null)
                    {
                        this.Executing(this, new EventArgs());
                    }

                    break;

                case "Idle":
                    this.AddMessage(string.Format("{0} has finished execution", this.ServiceApp.Name), ServiceAppState.Ready);

                    if (this.Executed != null)
                    {
                        this.Executed(this, new ServiceAppSuccessEventArgs(this.ServiceApp, DateTime.Now));
                    }

                    break;

                case "Stopped":
                    this.AddMessage(string.Format("{0} has stopped", this.ServiceApp.Name), ServiceAppState.NotLoaded);

                    if (this.Stopped != null)
                    {
                        this.Stopped(this, new ServiceAppStopEventArgs(ServiceApp.Name, false));
                    }

                    forceExit = true;
                    break;

                case "Failed":
                    this.AddMessage(string.Format("{0} has failed during execution", this.ServiceApp.Name), ServiceAppState.Error);

                    if (this.Failed != null)
                    {
                        this.Failed(this, new EventArgs());
                    }

                    break;
            }

            if (stateName == "Failed")
            {
                this._log.Warn(this.ServiceApp.LastMessage);
            }
            else
            {
                this._log.Info(this.ServiceApp.LastMessage);
            }

            return forceExit;
        }

        /// <summary>
        /// Creates the Process which will run the service app.
        /// </summary>
        private void CreateProcess()
        {
            _process = _processFactory.CreateProcess();
            int sacsProcessId = Process.GetCurrentProcess().Id;
            ProcessStartInfo startInfo = new ProcessStartInfo(ServiceApp.AppFilePath);
            startInfo.WorkingDirectory = Path.GetDirectoryName(ServiceApp.AppFilePath);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.ErrorDialog = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            if (ApplicationSettings.Current.EnableCustomUserLogins)
            {
                _process.ArgumentObject["name"] = this.ServiceApp.Name;
                _process.ArgumentObject["owner"] = sacsProcessId;
            }
            else
            {
                // keeping this older feature until testing is complete.
                startInfo.Arguments = JsonConvert.SerializeObject(new { name = ServiceApp.Name, action = "hide", owner = sacsProcessId });
            }

            if (!string.IsNullOrWhiteSpace(ServiceApp.Username))
            {
                startInfo.Domain = UserUtilities.GetDomain(ServiceApp.Username);
                startInfo.UserName = UserUtilities.GetUserName(ServiceApp.Username);
            }

            this._process.StartInfo = startInfo;
        }

        /// <summary>
        /// Starts the underlying process for the service app.
        /// </summary>
        private void StartProcess()
        {
            // only add the password when it's time to start the process.
            _process.StartInfo.Password = ServiceApp.Password.DecryptString(EntropyValue);
            bool hasError = false;

            try
            {
                // Update the version info here so that any issues with the app start are grouped together
                ServiceApp.RefreshAppVersion();
                AddMessage("Starting service app", ServiceAppState.Unknown);
                _process.Start();
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error starting service app {0}", ServiceApp.Name), e);
                AddMessage(e.Message, ServiceAppState.NotLoaded);
                hasError = true;
            }

            if (!hasError)
            {
                MonitorProcessCallback();
            }
        }

        /// <summary>
        /// Performs monitoring of the process (via the StandardOutput)
        /// </summary>
        private void MonitorProcess()
        {
            bool exitCheck = false;
            while (!exitCheck)
            {
                try
                {
                    string message = _process.StandardOutput.ReadLine();
                    exitCheck = ProcessMessage(message);
                }
                catch (ObjectDisposedException)
                {
                    // the stdout stream has been closed so we can leave this method
                    exitCheck = true;
                }
            }
        }

        /// <summary>
        /// Aborts the process in the service app.
        /// </summary>
        /// <returns></returns>
        private bool AbortProcess()
        {
            bool stopProcessingMessages;
            Stop(_canRecreateProcess);
            stopProcessingMessages = true;
            if (Stopped != null)
            {
                // If the process cannot be recreated, we don't care about an error.
                bool isError = _canRecreateProcess;
                Stopped(this, new ServiceAppStopEventArgs(ServiceApp.Name, isError));
            }

            return stopProcessingMessages;
        }

        #endregion Methods
    }
}