using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SACS.DataAccessLayer.Providers;
using SACS.Web.PresentationLogic.Providers;

namespace SACS.Web
{
    /// <summary>
    /// The entry point for the application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Handles the start of the application
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // used by the service app model
            ImagePathProvider.SetProvider(new WebImagePathProvider());
        }
    }
}
