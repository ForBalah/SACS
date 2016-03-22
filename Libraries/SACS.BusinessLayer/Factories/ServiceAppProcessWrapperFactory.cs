using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.BusinessLayer.Factories.Interfaces;
using SACS.Common.Factories.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.Factories
{
    /// <summary>
    /// Factory implementation that creates <see cref="ServiceAppProcess"/> instances that rely on
    /// <see cref="SACS.Common.Runtime.ProcessWrapper"/> to work.
    /// </summary>
    public class ServiceAppProcessWrapperFactory : IServiceAppProcessFactory
    {
        private IProcessWrapperFactory _processWrapperFactory;

        /// <summary>
        /// Instantiates a new instance of the <see cref="ServiceAppProcessWrapperFactory"/> class.
        /// </summary>
        /// <param name="processWrapperFactory">The <see cref="ProcessWrapper"/> factory to rely on.</param>
        public ServiceAppProcessWrapperFactory(IProcessWrapperFactory processWrapperFactory)
        {
            _processWrapperFactory = processWrapperFactory;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ServiceAppProcess"/>.
        /// </summary>
        /// <param name="app">The <see cref="ServiceApp"/> associated with the process.</param>
        /// <param name="log">The <see cref="ILog"/> that the service app process will use.</param>
        /// <returns>A new instance of <see cref="ServiceAppProcess"/>.</returns>
        public ServiceAppProcess CreateServiceAppProcess(ServiceApp app, ILog log)
        {
            return new ServiceAppProcess(app, log, _processWrapperFactory);
        }
    }
}
