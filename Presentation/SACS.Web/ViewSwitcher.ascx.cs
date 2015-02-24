using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls.Resolvers;

namespace SACS.Web
{
    /// <summary>
    /// The view switcher control.
    /// </summary>
    public partial class ViewSwitcher : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets the current view
        /// </summary>
        protected string CurrentView { get; private set; }

        /// <summary>
        /// Gets the alternate view
        /// </summary>
        protected string AlternateView { get; private set; }

        /// <summary>
        /// Gets the switch URL
        /// </summary>
        protected string SwitchUrl { get; private set; }

        /// <summary>
        /// Handles the load event of the page object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Determine current view
            var isMobile = WebFormsFriendlyUrlResolver.IsMobileView(new HttpContextWrapper(Context));
            this.CurrentView = isMobile ? "Mobile" : "Desktop";

            // Determine alternate view
            this.AlternateView = isMobile ? "Desktop" : "Mobile";

            // Create switch URL from the route, e.g. ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page
            var switchViewRouteName = "AspNet.FriendlyUrls.SwitchView";
            var switchViewRoute = RouteTable.Routes[switchViewRouteName];
            if (switchViewRoute == null)
            {
                // Friendly URLs is not enabled or the name of the switch view route is out of sync
                this.Visible = false;
                return;
            }

            var url = GetRouteUrl(switchViewRouteName, new { view = AlternateView, __FriendlyUrls_SwitchViews = true });
            url += "?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl);
            this.SwitchUrl = url;
        }
    }
}