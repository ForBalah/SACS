using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.Web.PresentationLogic.Fluent
{
    /// <summary>
    /// Wrapper class for the ServiceApp view/presenter
    /// </summary>
    public class ServiceAppWrapper
    {
        private readonly ServiceAppPresenter _presenter;
        private readonly ServiceAppView _view = new ServiceAppView();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppWrapper"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public ServiceAppWrapper(IRestClientFactory factory)
        {
            this._presenter = new ServiceAppPresenter(this._view, factory);
        }

        /// <summary>
        /// Gets the service application list.
        /// </summary>
        /// <value>
        /// The service application list.
        /// </value>
        public IList<DataAccessLayer.Models.ServiceApp> ServiceAppList
        {
            get
            {
                return this._view.ServiceAppList;
            }
        }

        /// <summary>
        /// Gets the logic exception.
        /// </summary>
        /// <value>
        /// The logic exception.
        /// </value>
        public Tuple<string, Exception> ExceptionItem
        {
            get
            {
                return this._view.Exception;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return this._view.Message;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return this.ExceptionItem == null;
            }
        }

        /// <summary>
        /// Loads the service apps.
        /// </summary>
        /// <returns></returns>
        public ServiceAppWrapper LoadServiceApps()
        {
            this._presenter.LoadServiceApps(true);
            return this;
        }

        /// <summary>
        /// Starts the specified service app.
        /// </summary>
        /// <param name="name">The service app name.</param>
        /// <returns></returns>
        public ServiceAppWrapper Start(string name)
        {
            this._presenter.StartServiceApp(new ServiceApp { Name = name });
            return this;
        }

        /// <summary>
        /// Stops the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ServiceAppWrapper Stop(string name)
        {
            this._presenter.StopServiceApp(new ServiceApp { Name = name });
            return this;
        }

        /// <summary>
        /// Runs the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ServiceAppWrapper Run(string name)
        {
            this._presenter.RunServiceApp(new ServiceApp { Name = name });
            return this;
        }
    }

    /// <summary>
    /// The view for this wrapper
    /// </summary>
    internal class ServiceAppView : IServiceAppView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppView"/> class.
        /// </summary>
        public ServiceAppView()
        {
            this.ServiceAppList = new List<DataAccessLayer.Models.ServiceApp>();
        }

        /// <summary>
        /// Gets the service application list.
        /// </summary>
        /// <value>
        /// The service application list.
        /// </value>
        public IList<DataAccessLayer.Models.ServiceApp> ServiceAppList { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the logic exception.
        /// </summary>
        /// <value>
        /// The logic exception.
        /// </value>
        public Tuple<string, Exception> Exception { get; private set; }

        /// <summary>
        /// Binds the service apps.
        /// </summary>
        /// <param name="list">The list.</param>
        public void BindServiceApps(IList<DataAccessLayer.Models.ServiceApp> list)
        {
            this.ServiceAppList = list ?? new List<DataAccessLayer.Models.ServiceApp>();
        }

        /// <summary>
        /// Selects the specified service app, or deselects if null is passed in
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        /// <exception cref="System.NotImplementedException">Not used in Web project.</exception>
        public void SelectServiceApp(DataAccessLayer.Models.ServiceApp serviceApp)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the status bar message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SetStatusMessage(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            this.Exception = new Tuple<string, Exception>(title, e);
        }
    }
}