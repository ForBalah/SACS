using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Entitites;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// The DAO for SACS system tasks
    /// </summary>
    public class SystemDao : GenericDao, ISystemDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDao"/> class.
        /// </summary>
        protected SystemDao()
            : base()
        {
        }

        /// <summary>
        /// Logs the system performances.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="cpuValue">The cpu value.</param>
        /// <param name="ramValue">The ram value.</param>
        public void LogSystemPerformances(string message, decimal? cpuValue, decimal? ramValue)
        {
            this.Insert(new SystemAudit
            {
                AuditType = AuditType.Performance,
                Message = message,
                CreatedByUserId = Environment.UserName,
                CreatedDate = DateTime.Now,
                CpuCounter = cpuValue.HasValue ? Math.Round(cpuValue.Value, 4) : (decimal?)null,
                RamCounter = ramValue.HasValue ? Math.Round(ramValue.Value, 4) : (decimal?)null
            });

            this.SubmitChanges();
        }

        /// <summary>
        /// Gets the cpu performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IList<Models.SystemPerformance> GetCpuPerformanceData(DateTime fromDate, DateTime toDate)
        {
            return (from audit in this.FindAll<SystemAudit>()
                    where audit.CreatedDate >= fromDate &&
                         audit.CreatedDate <= toDate &&
                         audit.AuditType == AuditType.Performance
                    select new Models.SystemPerformance
                    {
                        AuditTime = audit.CreatedDate,
                        Value = audit.CpuCounter ?? 0
                    }).AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets the memory performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IList<Models.SystemPerformance> GetMemoryPerformanceData(DateTime fromDate, DateTime toDate)
        {
            return (from audit in this.FindAll<SystemAudit>()
                    where audit.CreatedDate >= fromDate &&
                         audit.CreatedDate <= toDate &&
                         audit.AuditType == AuditType.Performance
                    select new Models.SystemPerformance
                    {
                        AuditTime = audit.CreatedDate,
                        Value = audit.RamCounter ?? 0
                    }).AsNoTracking().ToList();
        }
    }
}
