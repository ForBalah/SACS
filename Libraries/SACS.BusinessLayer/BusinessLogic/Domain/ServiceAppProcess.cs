using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Newtonsoft.Json;
using SACS.Common.Enums;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// The service app wrapper process wrapper
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

            ProcessStartInfo startInfo = new ProcessStartInfo(app.FullEntryFilePath);
            startInfo.Arguments = JsonConvert.SerializeObject(new { name = app.Name });
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            //startInfo.UserName = app.Username;
            //startInfo.Password = app.Password.

            this._process = new Process();
            this._process.StartInfo = startInfo;
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
                // TODO: surround with try/catch
                this._process.Start();
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
                            messageObject.error.exception.type as string,
                            messageObject.error.exception.message as string,
                            messageObject.error.exception.source as string,
                            messageObject.error.exception.stackTrace as string);

                        return false;
                    }
                    else if (messageObject.performance != null)
                    {
                        return false;
                    }
                    else if (messageObject.state != null)
                    {
                        this.ProcessState(messageObject.state.Value as string);
                        return false;
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
        /// <param name="message">The message.</param>
        /// <remarks>
        /// This may seem like a weird way of dealing with the exception, however
        /// at the time there was no known way of deserializing the exception correctly
        /// without going down the route of marshalling the exception and encoding it in
        /// base64 etc... That is still a viable option since this current method feels
        /// quite dirty.
        /// </remarks>
        private void ProcessError(string type, string message, string source, string stackTrace)
        {
            Exception tempEx = new Exception(string.Format("Message:{0} Source:{1}", message, source));
            this._log.Warn(string.Format("Uncaught error in {0}. Type: {1}", this.ServiceApp.Name, type), tempEx);

            if (this.Error != null)
            {
                this.Error(this, new ServiceAppErrorEventArgs(tempEx, this.ServiceApp.Name));
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
        private void ProcessState(string stateName)
        {
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
        }

        #endregion Methods
    }
}