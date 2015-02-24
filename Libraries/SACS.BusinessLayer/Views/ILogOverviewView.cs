using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// The Logs view
    /// </summary>
    public interface ILogOverviewView : IViewBase
    {
        /// <summary>
        /// Sets the current logs.
        /// </summary>
        /// <param name="logs">The logs.</param>
        void SetCurrentLogs(IEnumerable<string> logs);

        /// <summary>
        /// Clears the exception.
        /// </summary>
        void ClearException();
    }
}
