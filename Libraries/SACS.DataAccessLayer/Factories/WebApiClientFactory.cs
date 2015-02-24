using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Configuration;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.WebAPI;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.DataAccessLayer.Factories
{
    /// <summary>
    /// The factory used to create WebAPI clients
    /// </summary>
    public class WebApiClientFactory : IRestClientFactory
    {
        /// <summary>
        /// Add any new instance references here.
        /// </summary>
        private static Dictionary<Type, Type> classMap = new Dictionary<Type, Type>
            {
                { typeof(IServiceAppClient), typeof(ServiceAppClient) },
                { typeof(IAnalyticsClient), typeof(AnalyticsClient) },
                { typeof(ILogsClient), typeof(LogsClient) }
            };

        /// <summary>
        /// Creates the client, returning the reference as an interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>() where T : IWebApiClient
        {
            Type clientType = typeof(T);
            if (!classMap.ContainsKey(clientType))
            {
                throw new ArgumentException(string.Format("{0} could not be found in WebApiClientFactory", clientType.Name));
            }

            string basePath = ApplicationSettings.Current.WebApiBaseAddress;
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.UseDefaultCredentials = true;

            return (T)Activator.CreateInstance(
                        classMap[typeof(T)],
                        BindingFlags.NonPublic | BindingFlags.Public| BindingFlags.Instance,
                        default(Binder),
                        new object[] { basePath, clientHandler },
                        CultureInfo.InvariantCulture);
        }
    }
}
