using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SACS.Web.Startup))]

namespace SACS.Web
{
    /// <summary>
    /// The other startup partial class
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Performs the configuration using the app builder.
        /// </summary>
        /// <param name="app">The app builder</param>
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}