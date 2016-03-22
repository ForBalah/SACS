using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Application;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Models;

namespace SACS.WindowsService.WebAPI.Controllers
{
    /// <summary>
    /// The ServiceApp web API controller
    /// </summary>
    public class ServiceAppController : ApiController
    {
        private static ILog log = LogManager.GetLogger(typeof(ServiceAppController));
        private readonly IAppManager _appManager;
        private IServiceAppDao _dao = DaoFactory.Create<IServiceAppDao, ServiceAppDao>(); // TODO: move to DI
        private IAppListDao _appListDao = DaoFactory.Create<IAppListDao, AppListDao>(); // TODO: move to DI

        public ServiceAppController(IAppManager appManager)
        {
            _appManager = appManager;
        }

        /// <summary>
        /// Retrieve all the installed ServiceApps
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceApp> GetCurrentServiceApps()
        {
            return _appManager.ServiceApps;
        }

        /// <summary>
        /// Gets the service app history.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ServiceApp/MetricSummary")]
        public IDictionary<string, bool> GetServiceAppActiveHistory()
        {
            return this._dao.GetServiceAppActiveHistory();
        }

        /// <summary>
        /// Starts the service app with the specified app name
        /// </summary>
        /// <param name="id">The service app identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Start")]
        public IHttpActionResult StartServiceApp(string id)
        {
            string user = User != null && User.Identity != null ? User.Identity.Name : string.Empty;
            log.Debug("(API) Calling StartServiceApp - " + id);
            try
            {
                log.Info(string.Format("REQUEST ({0}) - START service app: {1}", user, id));
                string error = _appManager.InitializeServiceApp(id, this._dao);
                if (string.IsNullOrWhiteSpace(error))
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError(new AggregateException(error));
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Stops the service app with the specified app name
        /// </summary>
        /// <param name="id">The service app identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Stop")]
        public IHttpActionResult StopServiceApp(string id)
        {
            string user = User != null && User.Identity != null ? User.Identity.Name : string.Empty;
            log.Debug("(API) Calling StopServiceApp - " + id);
            try
            {
                log.Info(string.Format("REQUEST ({0}) - STOP service app immediately: {1}", user, id));
                _appManager.StopServiceApp(id, this._dao, false);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Runs the service app with the specified app name immediately, if it is started
        /// </summary>
        /// <param name="id">The service app identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Run")]
        public IHttpActionResult RunServiceApp(string id)
        {
            string user = User != null && User.Identity != null ? User.Identity.Name : string.Empty;
            log.Debug(string.Format("(API) Calling RunServiceApp ({0}) - {1}", user, id));
            try
            {
                if (_appManager.ServiceApps.Any(sa => sa.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    log.Info(string.Format("REQUEST ({0}) - RUN service app immediately: {1}", user, id));
                    _appManager.RunServiceApp(id);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Updates the service application.
        /// </summary>
        /// <param name="serviceApp">The service application.</param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("Update")]
        public IHttpActionResult UpdateServiceApp([FromBody] ServiceApp serviceApp)
        {
            string user = User != null && User.Identity != null ? User.Identity.Name : string.Empty;
            log.Debug("(API) Calling UpdateServiceApp - " + (serviceApp ?? new ServiceApp()).Name);
            try
            {
                log.Info(string.Format("REQUEST ({0}) - UPDATE service app: {1}", user, serviceApp.Name));
                string result = _appManager.UpdateServiceApp(serviceApp, this._dao, this._appListDao);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return BadRequest(result);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        /// <summary>
        /// Removes the service application.
        /// </summary>
        /// <param name="id">The service app identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("Remove")]
        public IHttpActionResult RemoveServiceApp(string id)
        {
            string user = User != null && User.Identity != null ? User.Identity.Name : string.Empty;
            log.Debug("(API) Calling RemoveServiceApp - " + id);
            try
            {
                if (_appManager.ServiceApps.Any(sa => sa.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    log.Info(string.Format("REQUEST ({0}) - DELETE service app: {1}", user, id));
                    _appManager.RemoveServiceApp(id, this._dao, this._appListDao);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
