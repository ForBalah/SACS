using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Extensions
{
    /// <summary>
    /// String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Truncates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <param name="replacement">The replacement when the string is truncated.</param>
        /// <returns></returns>
        public static string Truncate(this string value, int length, string replacement = null)
        {
            if (value == null)
            {
                return null;
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", "Truncate length cannot be less than zero.");
            }

            if (value.Length < length)
            {
                return value;
            }
            else
            {
                return string.Format("{0}{1}", value.Substring(0, length), replacement);
            }
        }
    }
}
