using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Runtime;

namespace SACS.Common.Factories.Interfaces
{
    /// <summary>
    /// Factory that creates ProcessWrapper instances
    /// </summary>
    public interface IProcessWrapperFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProcessWrapper"/>.
        /// </summary>
        /// <returns>A newly configured <see cref="ProcessWrapper"/>.</returns>
        ProcessWrapper CreateProcess();

        /// <summary>
        /// Refreshes the supplied <see cref="ProcessWrapper"/> by updating the information
        /// on it, or recreating it if it is stale
        /// </summary>
        /// <param name="processWrapper">The process to refresh.</param>
        void RefreshProcess(ProcessWrapper processWrapper);
    }
}
