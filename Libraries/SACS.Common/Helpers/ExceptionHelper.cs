using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Helpers
{
    /// <summary>
    /// Helper class for exception relation items
    /// </summary>
    public class ExceptionHelper
    {
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
