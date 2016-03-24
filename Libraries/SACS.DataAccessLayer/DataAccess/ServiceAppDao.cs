using System;
using System.Collections.Generic;
using System.Linq;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Entitites;
using SACS.DataAccessLayer.Models;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// DAO designed to work with ServiceApps
    /// </summary>
    public class ServiceAppDao : GenericDao, IServiceAppDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppDao"/> class.
        /// </summary>
        protected ServiceAppDao()
            : base()
        {
        }

        /// <summary>
        /// Saves the service app, updating the existing record, if found, or creating a new record.
        /// </summary>
        /// <param name="app">The service app.</param>
        /// <returns>The entropy value for the service app.</returns>
        public string SaveServiceApp(Models.ServiceApp app)
        {
            ServiceApplication appEntity = this.GetServiceApplication(app);

            bool created = false;

            if (appEntity == null)
            {
                appEntity = new ServiceApplication();
                appEntity.CreatedByUserId = Environment.UserName;
                appEntity.CreatedDate = DateTime.Now;
                this.Insert(appEntity);
                created = true;
            }

            // history is created before update
            this.CreateHistory(appEntity);

            appEntity.Name = app.Name;
            appEntity.Active = true;
            appEntity.CronSchedule = app.Schedule;
            appEntity.Description = app.Description;
            appEntity.Environment = app.Environment;
            appEntity.ModifiedByUserId = Environment.UserName;
            appEntity.ModifiedDate = DateTime.Now;
            appEntity.AppFilePath = app.AppFilePath;
            appEntity.SendSuccessNotification = app.SendSuccessNotification;
            appEntity.EntropyValue2 = app.EntropyValue2 ?? appEntity.EntropyValue2; // update entropy value only if it exists on the model

            AuditType appAuditType = created ? AuditType.Create : AuditType.Update;
            appEntity.ServiceApplicationAudits.Add(new ServiceApplicationAudit
            {
                AuditType = appAuditType,
                CreatedByUserId = Environment.UserName,
                CreatedDate = DateTime.Now,
                Message = app.LastMessage
            });

            SubmitChanges();

            return appEntity.EntropyValue2;
        }

        /// <summary>
        /// Records the perfromance for the service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="performance">The performance.</param>
        public void RecordPerfromance(string appName, AppPerformance performance)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);
            if (appEntity == null)
            {
                throw new KeyNotFoundException("ServiceApplication to log performance against could not be found");
            }

            var perfEntity = this.FindAll<ServiceApplicationPerfomance>(p => p.Guid == performance.Guid).FirstOrDefault();

            if (perfEntity == null)
            {
                perfEntity = new ServiceApplicationPerfomance
                {
                    ServiceApplication = appEntity,
                    Source = this.GetType().Name,
                    Guid = performance.Guid
                };
                this.Insert<ServiceApplicationPerfomance>(perfEntity);
            }

            perfEntity.StartTime = performance.StartTime;
            perfEntity.EndTime = performance.EndTime;
            perfEntity.Failed = performance.Failed;
            perfEntity.Message = performance.Message;

            this.SubmitChanges();
        }

        /// <summary>
        /// Records the service application start.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void RecordServiceAppStart(string appName)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);
            if (appEntity == null)
            {
                throw new KeyNotFoundException("ServiceApplication to log against could not be found");
            }

            this.Insert(new ServiceApplicationAudit
            {
                AuditType = AuditType.Start,
                CreatedByUserId = Environment.UserName,
                CreatedDate = DateTime.Now,
                Message = "Service started successfully.",
                ServiceApplication = appEntity
            });

            this.SubmitChanges();
        }

        /// <summary>
        /// Records the service application stop.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        public void RecordServiceAppStop(string appName)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);
            if (appEntity == null)
            {
                throw new KeyNotFoundException("ServiceApplication to log against could not be found");
            }

            this.Insert(new ServiceApplicationAudit
            {
                AuditType = AuditType.Stop,
                CreatedByUserId = Environment.UserName,
                CreatedDate = DateTime.Now,
                Message = "Service stopped successfully.",
                ServiceApplication = appEntity
            });

            this.SubmitChanges();
        }

        /// <summary>
        /// Gets the service apps active history.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, bool> GetServiceAppActiveHistory()
        {
            return this.FindAll<ServiceApplication>()
                .Select(a => new { a.Name, a.Active })
                .ToDictionary(item => item.Name, item => item.Active);
        }

        /// <summary>
        /// Deletes the service app.
        /// </summary>
        /// <param name="appName">Name of the app.</param>
        public void DeleteServiceApp(string appName)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);

            if (appEntity != null)
            {
                appEntity.Active = false;
                AuditType appAuditType = AuditType.Delete;
                appEntity.ServiceApplicationAudits.Add(new ServiceApplicationAudit
                {
                    AuditType = appAuditType,
                    CreatedByUserId = Environment.UserName,
                    CreatedDate = DateTime.Now,
                    Message = string.Empty
                });

                this.SubmitChanges();
            }
        }

        /// <summary>
        /// Gets the service application.
        /// </summary>
        /// <param name="app">The service app model.</param>
        /// <returns></returns>
        private ServiceApplication GetServiceApplication(Models.ServiceApp app)
        {
            return this.GetServiceApplication(app.Name);
        }

        /// <summary>
        /// Gets the service application.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <returns></returns>
        private ServiceApplication GetServiceApplication(string appName)
        {
            return this.FindAll<ServiceApplication>(sa => sa.Name.ToUpper() == appName.ToUpper()).FirstOrDefault();
        }

        /// <summary>
        /// Creates the history.
        /// </summary>
        /// <param name="appEntity">The application entity.</param>
        private void CreateHistory(ServiceApplication appEntity)
        {
            if (appEntity.Id != default(int))
            {
                this.Insert(new ServiceApplicationHistory
                {
                    Active = appEntity.Active,
                    CreatedByUserId = appEntity.CreatedByUserId,
                    CreatedDate = appEntity.CreatedDate,
                    CronSchedule = appEntity.CronSchedule,
                    Description = appEntity.Description,
                    Environment = appEntity.Environment,
                    AppFilePath = appEntity.AppFilePath,
                });
            }
        }
    }
}