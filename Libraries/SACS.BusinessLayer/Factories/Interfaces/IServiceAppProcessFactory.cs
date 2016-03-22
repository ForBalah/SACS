using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Domain;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.Factories.Interfaces
{
    /// <summary>
    /// Factory that creates <see cref="ServiceAppProcess"/> classes.
    /// </summary>
    public interface IServiceAppProcessFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="ServiceAppProcess"/>.
        /// </summary>
        /// <param name="app">The <see cref="ServiceApp"/> associated with the process.</param>
        /// <param name="log">The <see cref="ILog"/> that the service app process will use.</param>
        /// <returns>A new instance of <see cref="ServiceAppProcess"/>.</returns>
        ServiceAppProcess CreateServiceAppProcess(ServiceApp app, ILog log);
    }
}
