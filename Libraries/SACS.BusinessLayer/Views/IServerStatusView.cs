using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Enums;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// Server Status control view
    /// </summary>
    public interface IServerStatusView : IViewBase
    {
        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="serverStatus">The server status.</param>
        /// <param name="message">The message.</param>
        /// <param name="startTime">The server start time</param>
        /// <param name="e">The e.</param>
        void SetStatus(ServerStatus serverStatus, string message, DateTime? startTime, Exception e);

        /// <summary>
        /// Sets the type of the startup.
        /// </summary>
        /// <param name="startupType">Type of the startup.</param>
        void SetStartupType(string startupType);
    }
}
