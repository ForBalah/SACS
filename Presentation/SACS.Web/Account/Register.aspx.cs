using System;
using System.Linq;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using SACS.Web.Models;

namespace SACS.Web.Account
{
    /// <summary>
    /// The register page
    /// </summary>
    public partial class Register : Page
    {
        /// <summary>
        /// Handles the click event of the CreateUser object.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = new UserManager();
            var user = new ApplicationUser() { UserName = this.UserName.Text };
            IdentityResult result = manager.Create(user, this.Password.Text);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                this.ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}