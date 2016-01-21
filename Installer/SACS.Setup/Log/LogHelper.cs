using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Setup.Log
{
    /// <summary>
    /// Quick class to do setup logging with
    /// </summary>
    public class LogHelper
    {
        private static LogProvider _LogProvider;

        private LogHelper(Type origin)
        {
            this.OriginType = origin;
        }

        /// <summary>
        /// Gets or sets the log provider
        /// </summary>
        public static LogProvider LogProvider
        {
            get
            {
                if (_LogProvider == null)
                {
                    _LogProvider = new FileLogProvider(Path.Combine(Path.GetTempPath(), "SACS.Setup", "InstallLog.txt"));
                }

                return _LogProvider;
            }

            set
            {
                _LogProvider = value;
            }
        }

        public Type OriginType
        {
            get;
            private set;
        }

        public static LogHelper GetLogger(Type origin)
        {
            return new LogHelper(origin);
        }

        public static bool TryOpenLog()
        {
            try
            {
                _LogProvider.OpenLog();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, Exception e)
        {
            LogHelper.LogProvider.Log(this.OriginType, message, e);
        }
    }
}