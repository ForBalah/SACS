using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// View used for service apps
    /// </summary>
    public interface IServiceAppView : IViewBase
    {
        /// <summary>
        /// Binds the service apps.
        /// </summary>
        /// <param name="list">The list.</param>
        void BindServiceApps(IList<ServiceApp> list);

        /// <summary>
        /// Selects the specified service app, or deselects if null is passed in
        /// </summary>
        /// <param name="serviceApp">The service app.</param>
        void SelectServiceApp(ServiceApp serviceApp);

        /// <summary>
        /// Sets the status bar message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SetStatusMessage(string message);
    }
}
