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
        private ServiceApp _ServiceApp;
        private ILog _log;
        private Task _runTask;
        private bool _executeFlag = false;
        private bool _stopFlag = false;

        #endregion

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

            this._ServiceApp = app;
            this._log = log;
            this.Messages = new List<Tuple<string, ServiceAppState>>();

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

        #endregion

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
        /// Occurs when the service app has successfully stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the service app failed.
        /// </summary>
        public event EventHandler Failed;

        #endregion

        #region Properties

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
        /// Gets the last message.
        /// </summary>
        /// <value>
        /// The last message.
        /// </value>
        public string LastMessage
        {
            get
            {
                return this.Messages.Select(m => m.Item1).LastOrDefault();
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
                return this._ServiceApp;
            }
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Methods

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
        /// <returns></returns>
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
                    this.Messages.Add(new Tuple<string, ServiceAppState>(string.Format("{0} has started", this.ServiceApp.Name), ServiceAppState.Initialized));

                    if (this.Started != null)
                    {
                        this.Started(this, new EventArgs());
                    }

                    break;

                case "Executing":
                    this.Messages.Add(new Tuple<string, ServiceAppState>(string.Format("{0} is executing", this.ServiceApp.Name), ServiceAppState.Executing));

                    if (this.Executing != null)
                    {
                        this.Executing(this, new EventArgs());
                    }

                    break;

                case "Idle":
                    this.Messages.Add(new Tuple<string, ServiceAppState>(string.Format("{0} has finished execution", this.ServiceApp.Name), ServiceAppState.Initialized));

                    if (this.Executed != null)
                    {
                        this.Executed(this, new EventArgs());
                    }

                    break;

                case "Stopped":
                    this.Messages.Add(new Tuple<string, ServiceAppState>(string.Format("{0} has stopped", this.ServiceApp.Name), ServiceAppState.NotLoaded));

                    if (this.Stopped != null)
                    {
                        this.Stopped(this, new EventArgs());
                    }

                    break;

                case "Failed":
                    this.Messages.Add(new Tuple<string, ServiceAppState>(string.Format("{0} has failed during execution", this.ServiceApp.Name), ServiceAppState.Error));
 
                    if (this.Failed != null)
                    {
                        this.Failed(this, new EventArgs());
                    }

                    break;
            }

            if (stateName == "Failed")
            {
                this._log.Warn(this.LastMessage);
            }
            else
            {
                this._log.Info(this.LastMessage);
            }
        }

        #endregion
    }
}
