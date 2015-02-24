using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using SACS.Common.Configuration;
using SACS.WindowsService.WebAPI;

namespace SACS.WindowsService.Components
{
    /// <summary>
    /// The Web API communication component
    /// </summary>
    public class WebAPIComponent
    {
        private IDisposable _webApp;

        /// <summary>
        /// Starts this communication component
        /// </summary>
        public void Start()
        {
            if (this._webApp != null)
            {
                this._webApp.Dispose();
            }

            this._webApp = WebApp.Start<Startup>(url: ApplicationSettings.Current.WebApiBaseAddress);
        }

        /// <summary>
        /// Stops this communication component
        /// </summary>
        public void Stop()
        {
            if (this._webApp != null)
            {
                this._webApp.Dispose();
            }

            this._webApp = null;
        }
    }
}
