using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SACS.Web
{
    /// <summary>
    /// Web application loader file 
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Handles the start event of the application
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        public void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}