using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The Service apps controller
    /// </summary>
    public class ServiceAppsController : Controller
    {
        /// <summary>
        /// The Get action for the index.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}