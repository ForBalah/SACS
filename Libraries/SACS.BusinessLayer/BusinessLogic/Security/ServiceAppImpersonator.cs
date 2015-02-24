using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;

namespace SACS.BusinessLayer.BusinessLogic.Security
{
    /// <summary>
    /// The Impersonator wrapper for ServiceApps
    /// </summary>
    [Serializable]
    public class ServiceAppImpersonator
    {
        /// <summary>
        /// Runs the specified action as the given user
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="action">The action.</param>
        public virtual void RunAsUser(string username, string password, Action action)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                string[] parts = username.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                string domain = string.Empty;
                string name = username;
                if (parts.Length == 2)
                {
                    domain = parts[0];
                    name = parts[1];
                }

                string decryptedPassword = StringCipher.Decrypt(password, ApplicationSettings.Current.EncryptionSecretKey);

                using (Impersonator userImpersonator = new Impersonator(username, domain, decryptedPassword))
                {
                    action();
                }
            }
            else
            {
                // run as built-in user
                action();
            }
        }
    }
}
