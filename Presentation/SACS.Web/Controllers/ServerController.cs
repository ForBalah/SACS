using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACS.Common.Configuration;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The Server Controller
    /// </summary>
    public class ServerController : Controller
    {
        private readonly IRestClientFactory _factory = new WebApiClientFactory();
        private static string lookBackDaysSetting = ConfigurationManager.AppSettings["Performance.LookBackDays"];

        /// <summary>
        /// The index get action.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            this.SetConfigurationInfo();
            return View();
        }

        /// <summary>
        /// GET Cpu performance, using the day offset from now
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public ActionResult CpuData(double? offset)
        {
            double lookBackDays = double.Parse(lookBackDaysSetting, CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.Now.AddMinutes(1).AddDays(offset ?? 0);
            var perfClient = this._factory.Create<IAnalyticsClient>();
            var data = perfClient.GetSystemCpuPerformance(toDate.AddDays(-lookBackDays), toDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET Memory performance, using the day offset from now
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public ActionResult MemoryData(double? offset)
        {
            double lookBackDays = double.Parse(lookBackDaysSetting, CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.Now.AddMinutes(1).AddDays(offset ?? 0);
            var perfClient = this._factory.Create<IAnalyticsClient>();
            var data = perfClient.GetSystemMemoryPerformance(toDate.AddDays(-lookBackDays), toDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the configuration information.
        /// </summary>
        private void SetConfigurationInfo()
        {
            var appSettings = ApplicationSettings.Current;
            ViewBag.BaseAddress = appSettings.WebApiBaseAddress;
            ViewBag.PagingSize = appSettings.DefaultPagingSize;
            ViewBag.LookBackDays = lookBackDaysSetting;

            try
            {
                var serverClient = this._factory.Create<IServerClient>();
                ViewBag.Version = serverClient.GetVersionInfo();
            }
            catch
            {
            }

            try
            {
                var serverClient = this._factory.Create<IServerClient>();
                ViewBag.SupportEmail = serverClient.GetSupportEmailAddress();
            }
            catch
            {
            }
        }
    }
}