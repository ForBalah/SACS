using System;
using System.Linq;
using System.Net.Mail;
using log4net;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// Class providing standard web config-driven email sending capabilities.
    /// </summary>
    public class SmtpEmailProvider : EmailProvider
    {
        private ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpEmailProvider"/> class.
        /// </summary>
        /// <param name="log">The logger for the provider.</param>
        public SmtpEmailProvider(ILog log)
        {
            this._log = log;
        }

        /// <summary>
        /// Sends the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>true</c> if the mail sent successfully, else, <c>false</c>.</returns>
        public override bool Send(EmailMessage email)
        {
            if (email == null)
            {
                _log.Error("EmailMessage is null");
                return false;
            }

            _log.Debug(string.Format("EmailProvider.Send(). Subject: {0}. To: {1}", email.Subject, string.Join(",", email.ToAddresses)));

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(email.FromAddress);
                email.ToAddresses.ToList().ForEach(a =>
                {
                    message.To.Add(a);
                });
                email.CCAddresses.ToList().ForEach(a =>
                {
                    message.CC.Add(a);
                });

                message.Subject = email.Subject;
                message.IsBodyHtml = true;

                message.Body = email.Body;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Send(message);
                    _log.Info(string.Format("Sent '{0}' email to {1} and {2}", email.Subject, string.Join(",", email.ToAddresses), string.Join(",", email.CCAddresses)));
                }

                return true;
            }
            catch (SmtpException se)
            {
                _log.Error("Failed to send the email", se);
            }
            catch (FormatException)
            {
                string toAddresses = string.Join(", ", email.ToAddresses);
                _log.Error(string.Format("An email address was incorrect in mail message. From: {0}; To: {1}", email.FromAddress, toAddresses));
            }
            catch (InvalidOperationException ioe)
            {
                _log.Error("There was a problem preparing the message to send", ioe);
            }

            return false;
        }
    }
}
