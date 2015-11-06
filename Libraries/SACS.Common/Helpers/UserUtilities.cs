using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Helpers
{
    /// <summary>
    /// Helper class for processing user information
    /// </summary>
    public static class UserUtilities
    {
        /// <summary>
        /// Extracts the domain from the specified username.
        /// </summary>
        /// <param name="username">The username to get the domain from.</param>
        /// <returns></returns>
        public static string GetDomain(string username)
        {
            string[] parts = username.Split('\\');
            if (parts.Length == 2)
            {
                return parts[0];
            }

            parts = username.Split('@');
            if (parts.Length == 2)
            {
                return parts[1];
            }

            return string.Empty;
        }

        /// <summary>
        /// Extracts the user name part from the specified username.
        /// </summary>
        /// <param name="username">The username to get just the name from.</param>
        /// <returns></returns>
        public static string GetUserName(string username)
        {
            string[] parts = username.Split('\\');
            if (parts.Length == 2)
            {
                return parts[1];
            }

            parts = username.Split('@');
            if (parts.Length == 2)
            {
                return parts[0];
            }

            return username;
        }
    }
}