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
            var serviceApps = new ServiceAppWrapper(this._restFactory).LoadServiceApps();
            if (serviceApps.IsValid)
            {
                return View(serviceApps.ServiceAppList);
            }
            else
            {
                this.AddError(serviceApps.LogicException.Item2);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Run(string id)
        {
            return RedirectToAction("Index");
        }
    }
}