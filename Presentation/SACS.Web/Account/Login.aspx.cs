using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using SACS.Web.Models;

namespace SACS.Web.Account
{
    /// <summary>
    /// The login page
    /// </summary>
    public partial class Login : Page
    {
        /// <summary>
        /// Handles the load event of the Page object.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.RegisterHyperLink.NavigateUrl = "Register";
            this.OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!string.IsNullOrEmpty(returnUrl))
            {
                this.RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        /// <summary>
        /// Handles the login event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void LogIn(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                // Validate the user password
                var manager = new UserManager();
                ApplicationUser user = manager.Find(this.UserName.Text, this.Password.Text);
                if (user != null)
                {
                    IdentityHelper.SignIn(manager, user, this.RememberMe.Checked);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], this.Response);
                }
                else
                {
                    this.FailureText.Text = "Invalid username or password.";
                    this.ErrorMessage.Visible = true;
                }
            }
        }
    }
}