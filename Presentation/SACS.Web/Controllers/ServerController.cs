using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SACS.Common.Configuration;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.Web.Controllers
{
    /// <summary>
    /// The Server Controller
    /// </summary>
    public class ServerController : Controller
    {
        private readonly IRestClientFactory _factory = new WebApiClientFactory();

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
        /// Sets the configuration information.
        /// </summary>
        private void SetConfigurationInfo()
        {
            var appSettings = ApplicationSettings.Current;
            ViewBag.BaseAddress = appSettings.WebApiBaseAddress;
            ViewBag.PagingSize = appSettings.DefaultPagingSize;

            try
            {
                var serverClient = this._factory.Create<IServerClient>();
                ViewBag.Version = serverClient.GetVersionInfo();
            }
            catch
            {
            }
        }
    }
}