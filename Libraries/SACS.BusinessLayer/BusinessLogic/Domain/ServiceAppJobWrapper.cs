using System;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Security;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Models;
using SACS.Implementation;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Job wrapper for the ServiceAppBase execution
    /// </summary>
    internal class ServiceAppJobWrapper
    {
        private ServiceAppBase _serviceAppBase;
        private ServiceAppImpersonator _impersonator;
        private readonly string _appName;
        private readonly string _username;
        private readonly string _password;
        private readonly IServiceAppDao _dao;

        /// <summary>
        /// Occurs when an execution error occurs
        /// </summary>
        public event EventHandler<Exception> ExecutionError;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppJobWrapper" /> class.
        /// </summary>
        /// <param name="appBase">The application base.</param>
        /// <param name="dao">The DAO.</param>
        /// <param name="impersonator">The impersonator.</param>
        /// <param name="appName">Name of the application.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public ServiceAppJobWrapper(ServiceAppBase appBase, IServiceAppDao dao, ServiceAppImpersonator impersonator, string appName, string username, string password)
        {
            this._serviceAppBase = appBase;
            this._appName = appName;
            this._username = username;
            this._password = password;
            this._dao = dao;
            this._impersonator = impersonator;
        }

        #region Methods

        /// <summary>
        /// This method is invoked whenever the job is due for execution, after job initialization.
        /// </summary>
        public void Execute()
        {
            MessageProxy proxy = new MessageProxy(this._serviceAppBase, LogMessage);
            try
            {
                int performanceId = this._dao.RecordServiceAppExecutionStart(this._appName);
                this._impersonator.RunAsUser(this._username, this._password, () => this._serviceAppBase.Execute());
                this._dao.RecordServiceAppExecutionEnd(this._appName, performanceId);
            }
            catch (Exception e)
            {
                if (this.ExecutionError != null)
                {
                    this.ExecutionError(this, e);
                }
            }
            finally
            {
                proxy.RemoveMessageListener();
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void LogMessage(Message message)
        {
            string source = string.IsNullOrWhiteSpace(message.Source) ?
                this.GetType().Name :
                message.Source;

            source = string.Format("{0} ({1})", this._appName, source);

            ILog log = LogManager.GetLogger(source);

            if (message.Value is Exception)
            {
                log.Error(message.Value);
            }
            else
            {
                log.Info(message.Value);
            }
        }

        #endregion
    }
}
