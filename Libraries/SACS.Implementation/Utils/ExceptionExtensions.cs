using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Adds extra methods to the Exception class.
    /// </summary>
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the exception to its base64 representation.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static string ToBase64(this Exception exception)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(ms, exception);
                    ms.Position = 0;

                    return Convert.ToBase64String(ms.ToArray());
                }
                catch
                {
                    // Not a good idea, but the container will handle the rest.
                    return string.Empty;
                }
            }
        }
    }
}