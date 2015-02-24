using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// Base class that all DAOs should at inherit from
    /// </summary>
    public abstract class DaoBase
    {
        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        protected void LogMessage(string message, Exception e)
        {
            this.GetLog().Info(message, e);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        protected void LogWarning(string message, Exception e)
        {
            this.GetLog().Warn(message, e);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        protected void LogError(string message, Exception e)
        {
            this.GetLog().Error(message, e);
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <returns></returns>
        private ILog GetLog()
        {
            return LogManager.GetLogger(this.GetType());
        }
    }
}
