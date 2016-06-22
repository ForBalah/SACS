using System;
using SACS.Common.Enums;
using SACS.Common.Extensions;

namespace SACS.DataAccessLayer.Models
{
    /// <summary>
    /// The model representing a log entry. In hindsight this should have been in a separate models project.
    /// </summary>
    public class LogEntry
    {
        #region Properties

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public int Item { get; set; }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public LogImageType ImageType
        {
            get
            {
                switch (Level)
                {
                    case "ERROR":
                        return LogImageType.Error;
                    case "INFO":
                        return LogImageType.Info;
                    case "DEBUG":
                        return LogImageType.Debug;
                    case "WARN":
                        return LogImageType.Warn;
                    case "FATAL":
                        return LogImageType.Fatal;
                    default:
                        return LogImageType.Custom;
                }
            }
        }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the thread.
        /// </summary>
        /// <value>
        /// The thread.
        /// </value>
        public string Thread { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets the short version of the message
        /// </summary>
        public string ShortMessage
        {
            get
            {
                // TODO: The magic number should be in an appsetting, or a behaviour. But meh.
                return Message.Truncate(50, "...");
            }
        }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        /// <value>
        /// The name of the machine.
        /// </value>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public string App { get; set; }

        /// <summary>
        /// Gets or sets the throwable.
        /// </summary>
        /// <value>
        /// The throwable.
        /// </value>
        public string Throwable { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public string Line { get; set; } 

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether this instance contains the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public bool ContainsText(string text)
        {
            bool contains = false;

            contains |= (this.Message ?? string.Empty).IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;

            return contains;
        }

        #endregion
    }
}
