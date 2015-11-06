using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// This just helps to temporarily group common email functionality. This
    /// should be removed.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Sends the support email for the provided exception.
        /// </summary>
        /// <param name="emailer">The emailer.</param>
        /// <param name="ex">The exception to send.</param>
        /// <param name="serviceAppName">Name of the service application.</param>
        public static void SendSupportEmail(EmailProvider emailer, Exception ex, string serviceAppName)
        {
            string subjectName = !string.IsNullOrWhiteSpace(serviceAppName) ? string.Format(" ({0})", serviceAppName) : string.Empty;
            EmailTemplateProvider templater = new HtmlEmailTemplateProvider(ApplicationSettings.Current.SupportEmailTemplatePath);
            templater.AddValue("MachineName", Environment.MachineName);
            templater.AddValue("Message", ex.Message);
            templater.AddValue("StackTrace", ex.StackTrace);

            EmailMessage email = templater.GetEmailMessage();
            email.Subject = string.Format("{0}: Error in SACS{1}", Environment.MachineName, subjectName);
            email.FromAddress = ApplicationSettings.Current.SupportEmailAddress;
            email.AddToAddresses(ApplicationSettings.Current.SupportEmailAddress);

            emailer.Send(email);
        }
    }
}
