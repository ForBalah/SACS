using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SACS.Web.Models;

namespace SACS.Web.Account
{
    /// <summary>
    /// The register external login page.
    /// </summary>
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? string.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        /// <summary>
        /// Gets the provider account key
        /// </summary>
        protected string ProviderAccountKey
        {
            get { return (string)ViewState["ProviderAccountKey"] ?? string.Empty; }
            private set { ViewState["ProviderAccountKey"] = value; }
        }

        /// <summary>
        /// Handles the load event of the page object.
        /// </summary>
        protected void Page_Load()
        {
            // Process the result from an auth provider in the request
            this.ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (string.IsNullOrEmpty(this.ProviderName))
            {
                Response.Redirect("~/Account/Login");
            }

            if (!IsPostBack)
            {
                var manager = new UserManager();
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    Response.Redirect("~/Account/Login");
                }

                var user = manager.Find(loginInfo.Login);
                if (user != null)
                {
                    IdentityHelper.SignIn(manager, user, isPersistent: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else if (User.Identity.IsAuthenticated)
                {
                    // Apply Xsrf check when linking
                    var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                    if (verifiedloginInfo == null)
                    {
                        Response.Redirect("~/Account/Login");
                    }

                    var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                    if (result.Succeeded)
                    {
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    }
                    else
                    {
                        this.AddErrors(result);
                        return;
                    }
                }
                else
                {
                    this.userName.Text = loginInfo.DefaultUserName;
                }
            }
        }        
        
        /// <summary>
        /// Handles the Click event of the login object
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args</param>
        protected void LogIn_Click(object sender, EventArgs e)
        {
            this.CreateAndLoginUser();
        }

        /// <summary>
        /// Creates the user and automatically logs them in.
        /// </summary>
        private void CreateAndLoginUser()
        {
            if (!IsValid)
            {
                return;
            }

            var manager = new UserManager();
            var user = new ApplicationUser() { UserName = this.userName.Text };
            IdentityResult result = manager.Create(user);
            if (result.Succeeded)
            {
                var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
                if (loginInfo == null)
                {
                    Response.Redirect("~/Account/Login");
                    return;
                }

                result = manager.AddLogin(user.Id, loginInfo.Login);
                if (result.Succeeded)
                {
                    IdentityHelper.SignIn(manager, user, isPersistent: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                    return;
                }
            }

            this.AddErrors(result);
        }

        /// <summary>
        /// Adds the errors to the model state based on the identity result.
        /// </summary>
        /// <param name="result">The identity result</param>
        private void AddErrors(IdentityResult result) 
        {
            foreach (var error in result.Errors) 
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}