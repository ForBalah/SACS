using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace SACS.Web.Account
{
    /// <summary>
    /// The OAuth providers.
    /// </summary>
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the return URL
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Handles the load event of the Page object.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var provider = Request.Form["provider"];
                if (provider == null)
                {
                    return;
                }

                // Request a redirect to the external login provider
                string redirectUrl = ResolveUrl(string.Format(CultureInfo.InvariantCulture, "~/Account/RegisterExternalLogin?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, this.ReturnUrl));
                var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };

                // Add xsrf verification when linking accounts
                if (Context.User.Identity.IsAuthenticated)
                {
                    properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
                }

                Context.GetOwinContext().Authentication.Challenge(properties, provider);
                Response.StatusCode = 401;
                Response.End();
            }
        }

        /// <summary>
        /// Gets the provider names.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}