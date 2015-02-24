using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Entitites;

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
        public void SaveServiceApp(Models.ServiceApp app)
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
            appEntity.Active = app.StartupTypeEnum == Common.Enums.StartupType.Automatic;
            appEntity.AssemblyName = app.Assembly;
            appEntity.ConfigPath = app.ConfigFilePath;
            appEntity.CronSchedule = app.Schedule;
            appEntity.Description = app.Description;
            appEntity.Environment = app.Environment;
            appEntity.ModifiedByUserId = Environment.UserName;
            appEntity.ModifiedDate = DateTime.Now;
            appEntity.Path = app.Path;
            appEntity.EntryFile = app.EntryFile;

            AuditType appAuditType = created ? AuditType.Create : AuditType.Update;
            appEntity.ServiceApplicationAudits.Add(new ServiceApplicationAudit
            {
                AuditType = appAuditType,
                CreatedByUserId = Environment.UserName,
                CreatedDate = DateTime.Now,
                Message = app.LastMessage
            });

            this.SubmitChanges();
        }

        /// <summary>
        /// Records the execution start of the service app.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <returns>
        /// An integer reference to the performance record.
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">ServiceApplication to log against could not be found</exception>
        public int RecordServiceAppExecutionStart(string appName)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);
            if (appEntity == null)
            {
                throw new KeyNotFoundException("ServiceApplication to log against could not be found");
            }
            
            var performance = new ServiceApplicationPerfomance
            {
                ServiceApplication = appEntity,
                Source = this.GetType().Name,
                StartTime = DateTime.Now
            };

            this.Insert(performance);

            this.SubmitChanges();

            return performance.Id;
        }

        /// <summary>
        /// Records the service app execution end.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="performanceId">The performance identifier.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">ServiceApplication to log against could not be found</exception>
        public void RecordServiceAppExecutionEnd(string appName, int performanceId)
        {
            ServiceApplication appEntity = this.GetServiceApplication(appName);
            if (appEntity == null)
            {
                throw new KeyNotFoundException("ServiceApplication to log against could not be found");
            }

            var performance = this.FindAll<ServiceApplicationPerfomance>(p => p.Id == performanceId).FirstOrDefault();

            if (performance == null)
            {
                performance = new ServiceApplicationPerfomance
                {
                    ServiceApplication = appEntity,
                    Source = this.GetType().Name,
                    StartTime = DateTime.Now,
                    Message = string.Format("Original performance record {0} not found. Created new one.", performanceId)
                };
                this.Insert(performance);
            }

            performance.EndTime = DateTime.Now;

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
                    AssemblyName = appEntity.Name,
                    ConfigPath = appEntity.ConfigPath,
                    CreatedByUserId = appEntity.CreatedByUserId,
                    CreatedDate = appEntity.CreatedDate,
                    CronSchedule = appEntity.CronSchedule,
                    Description = appEntity.Description,
                    Environment = appEntity.Environment,
                    Path = appEntity.Path,
                    EntryFile = appEntity.EntryFile
                });
            }
        }
    }
}
