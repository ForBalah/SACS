using System;
using SACS.Common.Configuration;
using SACS.Common.Factories.Interfaces;
using SACS.Common.Runtime;

namespace SACS.Common.Factories
{
    /// <summary>
    /// Default implementation of <see cref="IProcessWrapperFactory"/>.
    /// </summary>
    public class ProcessWrapperFactory : IProcessWrapperFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProcessWrapper"/>.
        /// </summary>
        /// <returns>A newly configured <see cref="ProcessWrapper"/>.</returns>
        public ProcessWrapper CreateProcess()
        {
            bool enableCustomUserLogin = ApplicationSettings.Current.EnableCustomUserLogins;
            return new ProcessWrapper(enableCustomUserLogin);
        }

        /// <summary>
        /// Refreshes the supplied <see cref="ProcessWrapper"/> by updating the information
        /// on it if it is stale, or recreating it if it does not exist.
        /// </summary>
        /// <param name="processWrapper">The process to refresh.</param>
        public void RefreshProcess(ProcessWrapper processWrapper)
        {
            throw new NotImplementedException();
        }
    }
}
