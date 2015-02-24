using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SACS.Common.Enums;
using SACS.DataAccessLayer.Models;

namespace SACS.Windows.Extensions
{
    /// <summary>
    /// The extensions class for manipulating images in WPF
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// The log image list
        /// </summary>
        public static Dictionary<LogImageType, BitmapSource> LogImageList =
            new Dictionary<LogImageType, BitmapSource>()
            {
                {LogImageType.Debug, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Question.Handle, Int32Rect.Empty, null)},
                {LogImageType.Error, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Error.Handle, Int32Rect.Empty, null)},
                {LogImageType.Fatal, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Hand.Handle, Int32Rect.Empty, null)},
                {LogImageType.Info, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Information.Handle, Int32Rect.Empty, null)},
                {LogImageType.Warn, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Warning.Handle, Int32Rect.Empty, null)},
                {LogImageType.Custom, Imaging.CreateBitmapSourceFromHIcon(SystemIcons.Asterisk.Handle, Int32Rect.Empty, null)}
            };

        /// <summary>
        /// Gets the log image.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static BitmapSource GetLogImage(LogImageType type)
        {
            return LogImageList[type];
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="logEntry">The log entry.</param>
        /// <returns></returns>
        public static BitmapSource GetImage(this LogEntry logEntry)
        {
            return GetLogImage(logEntry.ImageType);
        }
    }
}
