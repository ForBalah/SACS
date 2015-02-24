using Owin;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;

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
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "WithActionApi",
                routeTemplate: "api/{controller}/{id}/{action}"
            );

            appBuilder.UseWebApi(config);
        }
    }
}