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
        /// <param name="p">The page number.</param>
        /// <param name="q">The search query.</param>
        /// <returns></returns>
        public ActionResult Detail(string id, int? p, string q)
        {
            var logEntries = new LogsWrapper(this._restFactory).LoadLogEntries(id, p ?? 0, q);
            if (logEntries.IsValid)
            {
                ViewBag.Title = id;
                return View(logEntries.LogEntries);
            }
            else
            {
                this.AddError(logEntries.ExceptionItem.Item2);
                return View("Error");
            }
        }
    }
}