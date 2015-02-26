using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SACS.Web.Models;

namespace SACS.Web.Models
{
    /// <summary>
    /// You can add User data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
    }

    /// <summary>
    /// The applications DB context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.Web.Models.ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }

    #region Helpers

    /// <summary>
    /// The User Manager.
    /// </summary>
    public class UserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.Web.Models.UserManager"/> class.
        /// </summary>
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }
    }
}

namespace SACS.Web
{
    /// <summary>
    /// Providers helper methods for Identity
    /// </summary>
    public static class IdentityHelper
    {
        /// <summary>
        /// Used for XSRF when linking external logins.
        /// </summary>
        public const string XsrfKey = "XsrfId";

        /// <summary>
        /// The provider name key.
        /// </summary>
        public const string ProviderNameKey = "providerName";

        /// <summary>
        /// Performs sign in.
        /// </summary>
        /// <param name="manager">The User Manager.</param>
        /// <param name="user">The applciation User.</param>
        /// <param name="isPersistent">Whether the connection is persistent.</param>
        public static void SignIn(UserManager manager, ApplicationUser user, bool isPersistent)
        {
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        /// <summary>
        /// Gets the provider name from the request.
        /// </summary>
        /// <param name="request">The http request.</param>
        /// <returns></returns>
        public static string GetProviderNameFromRequest(HttpRequest request)
        {
            return request[ProviderNameKey];
        }

        /// <summary>
        /// Gets the external login redirect URL.
        /// </summary>
        /// <param name="accountProvider">The name of the account provider.</param>
        /// <returns></returns>
        public static string GetExternalLoginRedirectUrl(string accountProvider)
        {
            return "/Account/RegisterExternalLogin?" + ProviderNameKey + "=" + accountProvider;
        }

        /// <summary>
        /// Gets a value indicating whether the URL is local.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns></returns>
        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }

        /// <summary>
        /// Use Response.Redirect to the return URL.
        /// </summary>
        /// <param name="returnUrl">The URL to return to.</param>
        /// <param name="response">The http response.</param>
        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!string.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }
    }
    #endregion
}