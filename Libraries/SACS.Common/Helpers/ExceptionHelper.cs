using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SACS.Common.Helpers
{
    /// <summary>
    /// Helper class for exception relation items
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Converts the exception to its base64 representation.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns></returns>
        public static string ConvertToBase64(Exception e)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(ms, e);
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

        /// <summary>
        /// Gets the formatted exception details.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns></returns>
        public static string GetExceptionDetails(Exception e)
        {
            StringBuilder detailsBuilder = new StringBuilder();
            detailsBuilder.AppendLine(GetMessageAndStackTrace(e));

            if (e.InnerException != null)
            {
                detailsBuilder.AppendLine();
                detailsBuilder.AppendLine(GetMessageAndStackTrace(e.InnerException));
            }

            return detailsBuilder.ToString();
        }

        /// <summary>
        /// Attempts to deserializes the exception string.
        /// </summary>
        /// <param name="serializedExeption">The base64 exception to deserialize.</param>
        /// <returns></returns>
        public static Exception DeserializeException(string serializedExeption)
        {
            try
            {
                byte[] array = Convert.FromBase64String(serializedExeption);
                using (var ms = new MemoryStream(array))
                {
                    var formatter = new BinaryFormatter();
                    return (Exception)formatter.Deserialize(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the message and stack trace for the specified exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns></returns>
        private static string GetMessageAndStackTrace(Exception e)
        {
            if (e == null)
            {
                return string.Empty;
            }

            return string.Format("{1}{0}{0}{2}", Environment.NewLine, e.Message, e.StackTrace);
        }
    }
}