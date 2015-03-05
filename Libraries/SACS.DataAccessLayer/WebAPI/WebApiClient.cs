using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using log4net;

namespace SACS.DataAccessLayer.WebAPI
{
    /// <summary>
    /// The Web API client
    /// </summary>
    public abstract class WebApiClient
    {
        private static ILog log = LogManager.GetLogger(typeof(WebApiClient));

        protected readonly string BaseAddress;
        protected readonly HttpMessageHandler ClientHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.DataAccessLayer.WebAPI.WebApiClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base Web API url.</param>
        /// <param name="httpMessageHandler">The message handler class dependency.</param>
        internal WebApiClient(string baseAddress, HttpMessageHandler httpMessageHandler)
        {
            this.BaseAddress = baseAddress;
            this.ClientHandler = httpMessageHandler;
        }

        /// <summary>
        /// Gets the specified resource path with the provided parameters
        /// </summary>
        /// <typeparam name="T">The type of resource to return.</typeparam>
        /// <param name="resourcePath">The resource path.</param>
        /// <param name="parameters">The request query parameters to add.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException">The REST exception.</exception>
        protected T Get<T>(string resourcePath, Dictionary<string, string> parameters = null)
        {
            string finalResourcePath = resourcePath;
            if (parameters != null)
            {
                finalResourcePath += "?" + string.Join("&", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)));
            }

            HttpResponseMessage response;
            string requestUri = string.Format("{0}/{1}", this.BaseAddress, finalResourcePath);

            using (HttpClient client = CreateHttpClient(new Uri(requestUri), this.ClientHandler))
            {
                try
                {
                    response = client.GetAsync(requestUri).Result;
                }
                catch (AggregateException aggregateException)
                {
                    Exception ex = GenerateException(requestUri, aggregateException, "GET");
                    log.Error("Error executing Get", ex);
                    throw ex;
                }
            }

            return ProcessResponse<T>(response, requestUri, "GET");
        }

        /// <summary>
        /// Posts the provided content to the specified path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcePath">The resource path.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        protected byte[] Post<T>(string resourcePath, T content)
        {
            HttpResponseMessage response;
            string requestUri = string.Format("{0}/{1}", this.BaseAddress, resourcePath);

            using (HttpClient client = CreateHttpClient(new Uri(requestUri), this.ClientHandler))
            {
                try
                {
                    response = client.PostAsync<T>(requestUri, content, new JsonMediaTypeFormatter()).Result;
                }
                catch (AggregateException aggregateException)
                {
                    Exception ex = GenerateException(requestUri, aggregateException, "POST");
                    log.Error("Error executing Post", ex);
                    throw ex;
                }
            }

            return ProcessResponse<byte[]>(response, requestUri, "POST");
        }

        /// <summary>
        /// Puts the provided content at the specified resource path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcePath">The resource path.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        protected byte[] Put<T>(string resourcePath, T content)
        {
            HttpResponseMessage response;
            string requestUri = string.Format("{0}/{1}", this.BaseAddress, resourcePath);

            using (HttpClient client = CreateHttpClient(new Uri(requestUri), this.ClientHandler))
            {
                try
                {
                    response = client.PutAsync<T>(requestUri, content, new JsonMediaTypeFormatter()).Result;
                }
                catch (AggregateException aggregateException)
                {
                    Exception ex = GenerateException(requestUri, aggregateException, "PUT");
                    log.Error("Error executing Put", ex);
                    throw ex;
                }
            }

            return ProcessResponse<byte[]>(response, requestUri, "PUT");
        }

        /// <summary>
        /// Delete the specified resource path.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcePath">The resource path.</param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException">The exception</exception>
        protected T Delete<T>(string resourcePath)
        {
            HttpResponseMessage response;
            string requestUri = string.Format("{0}/{1}", this.BaseAddress, resourcePath);

            using (HttpClient client = CreateHttpClient(new Uri(requestUri), this.ClientHandler))
            {
                try
                {
                    response = client.DeleteAsync(requestUri).Result;
                }
                catch (AggregateException aggregateException)
                {
                    Exception ex = GenerateException(requestUri, aggregateException, "DELETE");
                    log.Error("Error executing Get", ex);
                    throw ex;
                }
            }

            return ProcessResponse<T>(response, requestUri, "DELETE");
        }

        /// <summary>
        /// Creates the HTTP client.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="httpMessageHandler">The HTTP client handler.</param>
        /// <returns></returns>
        protected static HttpClient CreateHttpClient(Uri baseAddress, HttpMessageHandler httpMessageHandler)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = baseAddress.Host;
            uriBuilder.Scheme = baseAddress.Scheme;

            HttpClient client = new HttpClient(httpMessageHandler);
            client.Timeout = new TimeSpan(TimeSpan.TicksPerSecond * 30); // TODO: make this configurable
            client.BaseAddress = uriBuilder.Uri;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        /// <summary>
        /// Generates an exception based on the original request URI and aggregate exception
        /// </summary>
        /// <param name="requestUri">The original request URI</param>
        /// <param name="aggregateException">The aggregate exception to generate the final exception from</param>
        /// <param name="method">The REST method used.</param>
        /// <returns></returns>
        private static Exception GenerateException(string requestUri, AggregateException aggregateException, string method)
        {
            Exception generatedException;
            HttpRequestException httpRequestException = (HttpRequestException)aggregateException.InnerExceptions.Where(item => item is HttpRequestException).FirstOrDefault();
            if (httpRequestException != null)
            {
                generatedException = new HttpRequestException(string.Format("Error executing {0} for resource path '{1}'", method, requestUri), httpRequestException);
            }
            else
            {
                AggregateException flattenedException = aggregateException.Flatten();
                generatedException = flattenedException;
            }

            return generatedException;
        }

        /// <summary>
        /// Processes the response message and returns the results with the specified type
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="response">The Web API response message</param>
        /// <param name="requestUri">The original request Uri</param>
        /// <param name="method">The REST method used.</param>
        /// <returns></returns>
        private static T ProcessResponse<T>(HttpResponseMessage response, string requestUri, string method)
        {
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<T>().Result;
            }
            else
            {
                HttpRequestException ex = new HttpRequestException(string.Format("Error executing {3} for resource path '{0}': {1} - {2}", requestUri, (int)response.StatusCode, response.ReasonPhrase, method));
                log.Error("Failed to process HTTP response", ex);
                throw ex;
            }
        }
    }
}
