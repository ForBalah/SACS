using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.Factories.Interfaces
{
    /// <summary>
    /// Abstract factory pattern interface for web API access
    /// </summary>
    public interface IRestClientFactory
    {
        /// <summary>
        /// Creates the client, returning the reference as an interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>() where T : IWebApiClient;
    }
}
