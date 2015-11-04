using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// Extension methods for the Process class
    /// </summary>
    internal static class ProcessExtensions
    {
        /// <summary>
        /// Returns the parent of the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }

        /// <summary>
        /// Finds the name of the indexed process.
        /// </summary>
        /// <param name="pid">The pid.</param>
        /// <returns></returns>
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        /// <summary>
        /// Finds the pid from indexed process name.
        /// </summary>
        /// <param name="indexedProcessName">Name of the indexed process.</param>
        /// <returns></returns>
        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            try
            {
                return Process.GetProcessById((int)parentId.NextValue());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
