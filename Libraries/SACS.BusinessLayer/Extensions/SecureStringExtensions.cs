using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;

namespace SACS.BusinessLayer.Extensions
{
    /// <summary>
    /// SecureString Extension classes
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Encrypts the provided SecureString.
        /// </summary>
        /// <param name="input">The SecureString to encrypt.</param>
        /// <returns></returns>
        public static string EncryptString(this SecureString input)
        {
            if (input == null)
            {
                return null;
            }

            var encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(input.ToInsecureString()),
                ApplicationSettings.Current.EntropyValue,
                DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the provided data back into a SecureString.
        /// </summary>
        /// <param name="encryptedData">The data to decrypt.</param>
        /// <returns></returns>
        public static SecureString DecryptString(this string encryptedData)
        {
            if (encryptedData == null)
            {
                return null;
            }

            try
            {
                var decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    ApplicationSettings.Current.EntropyValue,
                    DataProtectionScope.CurrentUser);

                return Encoding.Unicode.GetString(decryptedData).ToSecureString();
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>
        /// Converts a string input to a SecureString.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns></returns>
        public static SecureString ToSecureString(this IEnumerable<char> input)
        {
            if (input == null)
            {
                return null;
            }

            var secure = new SecureString();

            foreach (var c in input)
            {
                secure.AppendChar(c);
            }

            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Converts the SecureString to a normal string.
        /// </summary>
        /// <param name="input">The SecureString to convert.</param>
        /// <returns></returns>
        public static string ToInsecureString(this SecureString input)
        {
            if (input == null)
            {
                return null;
            }

            var ptr = Marshal.SecureStringToBSTR(input);

            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}