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
    /// The Logs controller
    /// </summary>
    public class LogsController : Controller
    {
        private IRestClientFactory _restFactory = new WebApiClientFactory();

        /// <summary>
        /// GET log list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var logList = new LogsWrapper(this._restFactory).LoadLogList();
            if (logList.IsValid)
            {
                return View(logList.LogList);
            }
            else
            {
                this.AddError(logList.ExceptionItem.Item2);
                return View("Error");
            }
        }

        /// <summary>
        /// GET log detail
        /// </summary>
        /// <param name="id">The log name.</param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}