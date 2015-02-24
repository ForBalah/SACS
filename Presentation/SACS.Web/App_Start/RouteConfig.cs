using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace SACS.Web
{
    /// <summary>
    /// Route config
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Registers the routes
        /// </summary>
        /// <param name="routes">The routes</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
