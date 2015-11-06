using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Newtonsoft.Json;
using SACS.BusinessLayer.Extensions;
using SACS.Common.Enums;
using SACS.Common.Helpers;
using SACS.Common.Lock;
using SACS.DataAccessLayer.DataAccess.Interfaces;
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

        private Process _process;
        private ILog _log;
        private Task _runTask;
        private bool _executeFlag = false;
        private bool _stopFlag = false;
        private List<Tuple<string, ServiceAppState>> _Messages;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppProcess"/> class.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="log">The log.</param>
        /// <exception cref="System.ArgumentNullException">app;Cannot create ServiceAppProcess with null ServiceApp</exception>
        /// <exception cref="System.ArgumentException">Cannot create ServiceAppProcess with null or empty ServiceApp name</exception>
        internal ServiceAppProcess(ServiceApp app, ILog log)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app", "Cannot create ServiceAppProcess with null ServiceApp");
            }

            if (string.IsNullOrWhiteSpace(app.Name))
            {
                throw new ArgumentException("Cannot create ServiceAppProcess with null or empty ServiceApp name");
            }

            this.ServiceApp = app;
            this._log = log;
            this._Messages = new List<Tuple<string, ServiceAppState>>();

            this.CreateProcess();
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
        public event EventHandler Executed;

        /// <summary>
        /// Occurs when the service app generated an uncaught error.
        /// </summary>
        public event EventHandler<ServiceAppErrorEventArgs> Error;

        /// <summary>
        /// Occurs when the service app has successfully stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the service app failed.
        /// </summary>
        public event EventHandler Failed;

        #endregion Events

        #region Properties

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
        /// Gets a value indicating whether the process is currently running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.ServiceApp.CurrentState == ServiceAppState.Ready ||
                    this.ServiceApp.CurrentState == ServiceAppState.Executing ||
                    this.ServiceApp.CurrentState == ServiceAppState.Error;
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
        /// Gets the service app.
        /// </summary>
        /// <value>
        /// The service app.
        /// </value>
        public ServiceApp ServiceApp
        {
            get;
            private set;
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
        public virtual void Start()
        {
            this._runTask = Task.Run(() =>
            {
                bool exitCheck = false;

                try
                {
                    this._process.Start();
                }
                catch (Exception e)
                {
                    this._log.Error(string.Format("Error starting service app {0}", this.ServiceApp.Name), e);
                    this.AddMessage(e.Message, ServiceAppState.NotLoaded);
                    exitCheck = true;
                }

                while (!exitCheck)
                {
                    string message = this._process.StandardOutput.ReadLine();
                    exitCheck = this.ProcessMessage(message);
                }
            });
        }

        /// <summary>
        /// Stops the associated service app.
        /// </summary>
        public virtual void Stop()
        {
            this._process.StandardInput.WriteLine(JsonConvert.SerializeObject(new { action = "stop" }));

            // we will need to be doubly sure that the process has ended
            if (this.IsRunning || !this._process.HasExited)
            {
                int attempts = 5;
                while (attempts > 0)
                {
                    if (this.IsRunning || !this._process.HasExited)
                    {
                        Thread.Sleep(2000);
                        attempts--;
                    }
                    else
                    {
                        break;
                    }
                }

                if (attempts <= 0)
                {
                    this._process.Kill();
                    this.AddMessage("Service app forced stopped.", ServiceAppState.NotLoaded);
                }
            }

            if (!this.IsRunning)
            {
                this.CreateProcess();
            }
            else
            {
                throw new ApplicationException("Cannot re-create the service app's process");
            }
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if the process should exit after processing the message.</returns>
        /// <exception cref="System.NotImplementedException">The provided message has not handler.</exception>
        internal bool ProcessMessage(string message)
        {
            try
            {
                dynamic messageObject = JsonConvert.DeserializeObject(message);
                if (messageObject != null)
                {
                    if (messageObject.debug != null)
                    {
                        return false;
                    }
                    else if (messageObject.info != null)
                    {
                        this.ProcessInfo(messageObject.info.Value as string);
                        return false;
                    }
                    else if (messageObject.error != null)
                    {
                        this.ProcessError(
                            ExceptionHelper.DeserializeException(messageObject.error.exception.Value as string),
                            messageObject.error.details.type.Value as string,
                            messageObject.error.details.message.Value as string,
                            messageObject.error.details.source.Value as string,
                            messageObject.error.details.stackTrace.Value as string);

                        return false;
                    }
                    else if (messageObject.performance != null)
                    {
                        return false;
                    }
                    else if (messageObject.state != null)
                    {
                        return this.ProcessState(messageObject.state.Value as string);
                    }
                }
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                // this must be just a normal info message. process it as such
                this.ProcessInfo(message);
                return false;
            }

            throw new NotImplementedException();
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
            Exception finalException = exception ?? new Exception(string.Format("Message:{0} Source:{1}", message, source));
            this._log.Warn(string.Format("Uncaught error in {0}. Type: {1}", this.ServiceApp.Name, type), finalException);

            if (this.Error != null)
            {
                this.Error(this, new ServiceAppErrorEventArgs(finalException, this.ServiceApp.Name));
            }
        }

        /// <summary>
        /// Processes the information.
        /// </summary>
        /// <param name="info">The information.</param>
        private void ProcessInfo(string info)
        {
            this._log.Info(info);
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
                        this.Executed(this, new EventArgs());
                    }

                    break;

                case "Stopped":
                    this.AddMessage(string.Format("{0} has stopped", this.ServiceApp.Name), ServiceAppState.NotLoaded);

                    if (this.Stopped != null)
                    {
                        this.Stopped(this, new EventArgs());
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
            ProcessStartInfo startInfo = new ProcessStartInfo(this.ServiceApp.AppFilePath);
            startInfo.Arguments = JsonConvert.SerializeObject(new { name = this.ServiceApp.Name, action = "hide" });
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            if (!string.IsNullOrWhiteSpace(this.ServiceApp.Username))
            {
                startInfo.Domain = UserUtilities.GetDomain(this.ServiceApp.Username);
                startInfo.UserName = UserUtilities.GetUserName(this.ServiceApp.Username);
                startInfo.Password = this.ServiceApp.Password.DecryptString();
            }

            this._process = new Process();
            this._process.StartInfo = startInfo;
        }

        #endregion Methods
    }
}