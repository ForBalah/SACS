using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SACS.BusinessLayer.BusinessLogic.Email
{
    /// <summary>
    /// Class providing base email sending capabilities. TODO: make this abstract
    /// </summary>
    public class EmailProvider
    {
        private static ILog _log = LogManager.GetLogger(typeof(EmailProvider));

        /// <summary>
        /// Sends the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns><c>true</c> if the mail sent successfully, else, <c>false</c>.</returns>
        public virtual bool Send(EmailMessage email)
        {
            if (email == null)
            {
                _log.Error("EmailMessage is null");
                return false;
            }

            try
            {
                MailMessage message = new System.Net.Mail.MailMessage();
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

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Send(message);
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
