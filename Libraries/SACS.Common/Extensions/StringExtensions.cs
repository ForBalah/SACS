using System;

namespace SACS.Common.Extensions
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
                return string.Format("{0}{1}", value.RemoveNewLines(true).Substring(0, length), replacement);
            }
        }

        /// <summary>
        /// Returns the replacement string if the specified string is null or empty, otherwise it will return the string
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string IfNullOrEmpty(this string value, string replacement)
        {
            if (string.IsNullOrEmpty(value))
            {
                return replacement;
            }

            return value;
        }

        /// <summary>
        /// Removes new lines from the specified string.
        /// </summary>
        /// <param name="value">The string to clean.</param>
        /// <param name="addSpace">Whether single spaces should replace the new lines.</param>
        /// <returns></returns>
        public static string RemoveNewLines(this string value, bool addSpace)
        {
            if (value == null)
            {
                return value;
            }

            return value.Replace("\r", string.Empty).Replace("\n", addSpace ? " " : string.Empty);
        }
    }
}