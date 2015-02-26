using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using SACS.Web.Models;

namespace SACS.Web.Account
{
    /// <summary>
    /// The user manager page
    /// </summary>
    public partial class Manage : System.Web.UI.Page
    {
        /// <summary>
        /// Gets the success message
        /// </summary>
        protected string SuccessMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether external logins can be removed
        /// </summary>
        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the specifies user manager has a password
        /// </summary>
        /// <param name="manager">The User Manager</param>
        /// <returns></returns>
        private bool HasPassword(UserManager manager)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            return user != null && user.PasswordHash != null;
        }

        /// <summary>
        /// Handles the load event of the Page object
        /// </summary>
        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                // Determine the sections to render
                UserManager manager = new UserManager();
                if (this.HasPassword(manager))
                {
                    this.changePasswordHolder.Visible = true;
                }
                else
                {
                    this.setPassword.Visible = true;
                    this.changePasswordHolder.Visible = false;
                }

                this.CanRemoveExternalLogins = manager.GetLogins(User.Identity.GetUserId()).Count() > 1;

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    this.SuccessMessage =
                        message == "ChangePwdSuccess" ? "Your password has been changed."
                        : message == "SetPwdSuccess" ? "Your password has been set."
                        : message == "RemoveLoginSuccess" ? "The account was removed."
                        : string.Empty;
                    this.successMessage.Visible = !string.IsNullOrEmpty(this.SuccessMessage);
                }
            }
        }

        /// <summary>
        /// Handles the click event of the ChangePassword object
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                UserManager manager = new UserManager();
                IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), this.CurrentPassword.Text, this.NewPassword.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                }
                else
                {
                    this.AddErrors(result);
                }
            }
        }

        /// <summary>
        /// Handles the click event of the SetPassword object.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                UserManager manager = new UserManager();
                IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), this.password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    this.AddErrors(result);
                }
            }
        }

        /// <summary>
        /// Gets the list of login infos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserLoginInfo> GetLogins()
        {
            UserManager manager = new UserManager();
            var accounts = manager.GetLogins(User.Identity.GetUserId());
            this.CanRemoveExternalLogins = accounts.Count() > 1 || this.HasPassword(manager);
            return accounts;
        }

        /// <summary>
        /// Removes the login for the specified user.
        /// </summary>
        /// <param name="loginProvider">The login provider</param>
        /// <param name="providerKey">The provider key</param>
        public void RemoveLogin(string loginProvider, string providerKey)
        {
            UserManager manager = new UserManager();
            var result = manager.RemoveLogin(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            var msg = result.Succeeded
                ? "?m=RemoveLoginSuccess"
                : string.Empty;
            Response.Redirect("~/Account/Manage" + msg);
        }

        /// <summary>
        /// Adds errors to the model state.
        /// </summary>
        /// <param name="result">The identity result.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}