﻿using System.Web.Http;
using Autofac.Integration.WebApi;
using Owin;

namespace SACS.WindowsService.WebAPI
{
    /// <summary>
    /// The entry class for web API start up.
    /// </summary>
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            // NB!! Always put the most restrictive routes first
            config.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "api/{controller}/{id}/{action}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // be sure to link up DI (do confirm that the controllers are registered in the container)
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Program.Container);

            appBuilder.UseWebApi(config);
        }
    }
}