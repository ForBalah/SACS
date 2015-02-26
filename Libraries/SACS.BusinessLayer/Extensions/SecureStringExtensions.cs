using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Extensions
{
    /// <summary>
    /// SecureString Extension classes
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Converts the existing secure string to a managed string. Try not to use this often since that
        /// will end up negating the need for unmanaged secure string values
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        /// <returns></returns>
        public static string ToManagedString(this SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Converts the managed string to a <see cref="System.Security.SecureString" /> for login sensitive usage.
        /// </summary>
        /// <param name="managedString">The managed string.</param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string managedString)
        {
            var secureStr = new SecureString();
            if (managedString.Length > 0)
            {
                foreach (var c in managedString.ToCharArray())
                {
                    secureStr.AppendChar(c);
                }
            }

            return secureStr;
        }
    }
}
