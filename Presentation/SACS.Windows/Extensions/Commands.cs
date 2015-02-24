using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SACS.Windows.Extensions
{
    /// <summary>
    /// Provides a set of SACS Windows commands
    /// </summary>
    public static class Commands
    {
        private static RoutedUICommand export = new RoutedUICommand("Export", "Export", typeof(Commands));
        private static RoutedUICommand logs = new RoutedUICommand("Logs", "Logs", typeof(Commands));
        private static RoutedUICommand analytics = new RoutedUICommand("Analytics", "Analytics", typeof(Commands));

        /// <summary>
        /// Gets the value that represents the export list command
        /// </summary>
        /// <value>
        /// The export list.
        /// </value>
        public static RoutedUICommand ExportList
        {
            get
            {
                return export;
            }
        }

        /// <summary>
        /// Gets the value that represents the view logs command
        /// </summary>
        /// <value>
        /// The view logs.
        /// </value>
        public static RoutedUICommand ViewLogs
        {
            get
            {
                return logs;
            }
        }

        /// <summary>
        /// Gets the value that represents the view analytics command
        /// </summary>
        /// <value>
        /// The view logs.
        /// </value>
        public static RoutedUICommand ViewAnalytics
        {
            get
            {
                return analytics;
            }
        }
    }
}
