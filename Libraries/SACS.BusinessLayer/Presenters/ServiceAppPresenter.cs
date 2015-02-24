using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Export;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// Presenter used for ServiceApp view
    /// </summary>
    public class ServiceAppPresenter : PresenterBase<IServiceAppView>
    {
        private readonly IRestClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppPresenter" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="factory">The factory.</param>
        public ServiceAppPresenter(IServiceAppView view, IRestClientFactory factory)
            : base(view)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Exports the service application list.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        /// The exported list
        /// </returns>
        /// <exception cref="System.NotSupportedException">Thrown when the supplied extension has not export method defined.</exception>
        public string ExportServiceAppList(string extension)
        {
            IServiceAppClient client = this.factory.Create<IServiceAppClient>();
            IList<ServiceApp> serviceAppList = client.GetInstalledServiceApps();

            string exportContents;
            ServiceAppListExporter listExporter = new ServiceAppListExporter();
            if (extension.Contains("txt"))
            {
                exportContents = listExporter.ExportTabDelimited(serviceAppList);
            }
            else if (extension.Contains("csv"))
            {
                exportContents = listExporter.ExportCsv(serviceAppList);
            }
            else
            {
                throw new NotSupportedException(string.Format("The {0} extension is not supported."));
            }

            return exportContents;
        }

        /// <summary>
        /// Loads the service apps.
        /// </summary>
        /// <param name="isViewVisible">if set to <c>true</c> [is view visible].</param>
        public void LoadServiceApps(bool isViewVisible)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                this.View.BindServiceApps(client.GetInstalledServiceApps());
                this.View.SetStatusMessage(null);
            }, 
            () =>
            {
                this.View.BindServiceApps(new List<ServiceApp>());
            },
            isViewVisible);
        }

        /// <summary>
        /// Loads the service apps.
        /// </summary>
        public void LoadServiceApps()
        {
            this.LoadServiceApps(true);
        }

        /// <summary>
        /// Starts the service application.
        /// </summary>
        /// <param name="serviceApp">The service app to start.</param>
        public void StartServiceApp(ServiceApp serviceApp)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                client.StartServiceApp(serviceApp.Name);
            }, true);

            this.LoadServiceApps();
        }

        /// <summary>
        /// Stops the service application.
        /// </summary>
        /// <param name="serviceApp">The service app to stop.</param>
        public void StopServiceApp(ServiceApp serviceApp)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                client.StopServiceApp(serviceApp.Name);
            }, true);

            this.LoadServiceApps();
        }

        /// <summary>
        /// Runs the service application
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        public void RunServiceApp(ServiceApp serviceApp)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                client.RunServiceApp(serviceApp.Name);
            }, true);

            this.LoadServiceApps();
        }

        /// <summary>
        /// Adds or Updates the service application.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        public void UpdateServiceApp(ServiceApp serviceApp)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                client.UpdateServiceApp(serviceApp);
            }, true);

            this.LoadServiceApps();
            this.View.SelectServiceApp(serviceApp);
        }

        /// <summary>
        /// Removes the service application.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void RemoveServiceApp(string appName)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                client.RemoveServiceApp(appName);
            }, true);

            this.LoadServiceApps();
        }

        /// <summary>
        /// Gets the service application details.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void GetServiceAppDetails(string appName)
        {
            this.TryExecute(() =>
            {
                IServiceAppClient client = this.factory.Create<IServiceAppClient>();
                this.View.SelectServiceApp(client.GetServiceApp(appName));
            }, true);
        }

        /// <summary>
        /// Peforms the specified action, wrapped in a try/catch and informs the view of the outcome
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="showError">if set to <c>true</c> show the error in the view.</param>
        private void TryExecute(Action action, bool showError)
        {
            TryExecute(action, null, showError);
        }
    }
}
