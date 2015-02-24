using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using SACS.DataAccessLayer.Models;
using SACS.Windows.Extensions;

namespace SACS.Windows.ViewModels
{
    /// <summary>
    /// ViewModel for log entry
    /// </summary>
    public class LogEntryViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryViewModel"/> class.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        public LogEntryViewModel(LogEntry logEntry)
        {
            this.LogEntry = logEntry;
        }

        /// <summary>
        /// Gets the log entry.
        /// </summary>
        /// <value>
        /// The log entry.
        /// </value>
        public LogEntry LogEntry { get; private set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public BitmapSource Image
        {
            get
            {
                return this.LogEntry.GetImage();
            }
        }
    }
}
