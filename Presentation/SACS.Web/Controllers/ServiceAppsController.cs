using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.Web.PresentationLogic.Extensions;
using SACS.Web.PresentationLogic.Fluent;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The Service apps controller
    /// </summary>
    public class ServiceAppsController : Controller
    {
        private IRestClientFactory _restFactory = new WebApiClientFactory();

        /// <summary>
        /// The Get action for the index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var appWrapper = new ServiceAppWrapper(this._restFactory).LoadServiceApps();
            if (appWrapper.IsValid)
            {
                return View(appWrapper.ServiceAppList);
            }
            else
            {
                this.AddError(appWrapper.ExceptionItem.Item2);
                return View("Error");
            }
        }

        /// <summary>
        /// Metricses the specified from.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns></returns>
        public ActionResult Metrics(string from = null, string to = null)
        {
            return View();
        }

        /// <summary>
        /// Updates the specified service app based on the process.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string id, string process)
        {
            var appWrapper = new ServiceAppWrapper(this._restFactory);
            string result = string.Empty;
            switch (process)
            {
                case "Start":
                    appWrapper.Start(id);
                    result = appWrapper.IsValid ? "successStart" : "error";
                    break;
                case "Stop":
                    appWrapper.Stop(id);
                    result = appWrapper.IsValid ? "successStop" : "error";
                    break;
                case "Run":
                    appWrapper.Run(id);
                    result = appWrapper.IsValid ? "successRun" : "error";
                    break;
            }

            // TODO: log any exceptions to elmah
            return new RedirectResult(Url.Action("Index") + "#" + result);
        }
    }
}