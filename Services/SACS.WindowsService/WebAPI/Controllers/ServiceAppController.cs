using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
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
        private IServiceAppDao _dao = DaoFactory.Create<IServiceAppDao, ServiceAppDao>();
        private IAppListDao _appListDao = DaoFactory.Create<IAppListDao, AppListDao>();

        /// <summary>
        /// Retrieve all the installed ServiceApps
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceApp> GetCurrentServiceApps()
        {
            return AppManager.Current.ServiceApps;
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
            try
            {
                string error = AppManager.Current.InitializeServiceApp(id, this._dao);
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
            try
            {
                AppManager.Current.StopServiceApp(id, this._dao);
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
            try
            {
                if (AppManager.Current.ServiceApps.Any(sa => sa.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    AppManager.Current.RunServiceApp(id, this._dao);
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
            try
            {
                string result = AppManager.Current.UpdateServiceApp(serviceApp, this._dao, this._appListDao);
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
            try
            {
                if (AppManager.Current.ServiceApps.Any(sa => sa.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase)))
                {
                    AppManager.Current.RemoveServiceApp(id, this._dao, this._appListDao);
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
