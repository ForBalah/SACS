using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Adds extra methods to the Exception class
    /// </summary>
    internal static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the exception to it's base64 representation
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static string ToBase64(this Exception exception)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, exception);
                ms.Position = 0;

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
