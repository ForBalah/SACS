using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SACS.Setup.Log
{
    /// <summary>
    /// Simple file log provider
    /// </summary>
    public class FileLogProvider : LogProvider
    {
        private FileInfo _logFileInfo;

        public FileLogProvider(string logName)
            : base()
        {
            _logFileInfo = new FileInfo(logName);
        }

        public override void Log(Type type, string message, Exception e)
        {
            string log = string.Format(
                "[{0}] {1}: {2}{3}{4}",
                DateTime.Now,
                type != null ? type.ToString() : "<no class specified>",
                message ?? "<no message provided>",
                Environment.NewLine,
                e != null ? string.Format("{0}: {1}{2}", e.Message, e.StackTrace, Environment.NewLine) : string.Empty);

            int tries = 5;
            while (tries > 0)
            {
                try
                {
                    if (!Directory.Exists(this._logFileInfo.DirectoryName))
                    {
                        Directory.CreateDirectory(this._logFileInfo.DirectoryName);
                    }

                    File.AppendAllText(this._logFileInfo.FullName, log);
                    tries = 0;
                }
                catch
                {
                    Thread.Sleep(2000);
                    tries--;
                }
            }
        }

        public override void OpenLog()
        {
            Process.Start(this._logFileInfo.FullName);
        }
    }
}