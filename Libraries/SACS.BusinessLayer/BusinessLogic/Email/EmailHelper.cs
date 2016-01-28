using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;
using SACS.Scheduler;

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
            string source = !string.IsNullOrWhiteSpace(serviceAppName) ? string.Format("Service App: {0}", serviceAppName) : "SACS Server";
            EmailTemplateProvider templater = new HtmlEmailTemplateProvider(ApplicationSettings.Current.SupportEmailTemplatePath);
            templater.AddValue("MachineName", Environment.MachineName);
            templater.AddValue("Message", ex.Message);
            templater.AddValue("StackTrace", ex.StackTrace);
            templater.AddValue("Time", SystemTime.Now.ToString());
            templater.AddValue("Source", source);

            EmailMessage email = templater.GetEmailMessage();
            email.Subject = string.Format("{0}: Error in SACS{1}", Environment.MachineName, subjectName);
            email.FromAddress = ApplicationSettings.Current.SupportEmailAddress; // TODO: use SMTP settings instead?
            email.AddToAddresses(ApplicationSettings.Current.SupportEmailAddress);

            emailer.Send(email);
        }

        /// <summary>
        /// Sends a success email
        /// </summary>
        /// <param name="emailer">The emailer</param>
        /// <param name="serviceAppName">The name of the service app.</param>
        /// <param name="environment">The environment to put in the email message.</param>
        /// <param name="dateTime">The time execution completed.</param>
        public static void SendSuccessEmail(EmailProvider emailer, string serviceAppName, string environment, DateTime dateTime)
        {
            string subjectName = !string.IsNullOrWhiteSpace(serviceAppName) ? string.Format(" ({0})", serviceAppName) : string.Empty;
            EmailTemplateProvider templater = new HtmlEmailTemplateProvider(ApplicationSettings.Current.SuccessEmailTemplatePath);
            templater.AddValue("MachineName", Environment.MachineName);
            templater.AddValue("AppName", serviceAppName);
            templater.AddValue("Environment", environment);
            templater.AddValue("FinishTime", dateTime.ToString());

            EmailMessage email = templater.GetEmailMessage();
            email.Subject = string.Format("{0}: {1} ({2}) has successfully executed", Environment.MachineName, serviceAppName, environment);
            email.FromAddress = ApplicationSettings.Current.SupportEmailAddress;
            email.AddToAddresses(ApplicationSettings.Current.SupportEmailAddress);

            emailer.Send(email);
        }
    }
}