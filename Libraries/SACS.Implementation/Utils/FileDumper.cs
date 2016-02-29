using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// The class that deals with dumping service app contents to a file
    /// </summary>
    public class FileDumper
    {
        /// <summary>
        /// The format of the string to use during dumping
        /// </summary>
        private const string DumpFormat = "{0} - {1} - {2}\r\n  CPU Time: {3}\r\n  Memory: {4} KB\r\n";

        /// <summary>
        /// The name of the dump file
        /// </summary>
        private const string FileName = "dump.txt";

        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Dumps the contents of the exception to the dump file
        /// </summary>
        /// <param name="e">The exception to dump</param>
        public static void Dump(Exception e)
        {
            var innerEx = e.InnerException != null ?
                string.Format(Environment.NewLine + " Inner exception: {0} {1}", e.InnerException.Message, e.InnerException.StackTrace) :
                string.Empty;
            Dump(string.Format("{0} {1}{2}", e.Message, e.StackTrace, innerEx));
        }

        /// <summary>
        /// Dumps the contents of the message to the dump file
        /// </summary>
        /// <param name="message">The message to dump in the file</param>
        public static void Dump(string message)
        {
            if (Settings.DumpToFile)
            {
                var stackTrace = new StackTrace();
                var methodName = string.Empty;
                foreach (var frame in stackTrace.GetFrames())
                {
                    methodName = string.Format("{0}.{1}()", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name);
                    if (frame.GetMethod().Name != "Dump")
                    {
                        break;
                    }
                }

                var process = Process.GetCurrentProcess();
                var dumpTime = DateTime.Now;
                mutex.WaitOne(5000);
                try
                {
                    var cpu = process.UserProcessorTime;
                    var ram = process.WorkingSet64 / 1024m;
                    var dumpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FileName);
                    File.AppendAllText(dumpFile, string.Format(DumpFormat, dumpTime, methodName, message, cpu, ram));
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}