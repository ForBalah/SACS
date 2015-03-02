using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The Server Controller
    /// </summary>
    public class ServerController : Controller
    {
        /// <summary>
        /// The index get action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}