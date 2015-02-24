﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Entitites;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// DAO designed to work with App Performance data
    /// </summary>
    public class AppPerformanceDao : GenericDao, IAppPerformanceDao
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppDao"/> class.
        /// </summary>
        protected AppPerformanceDao()
            : base()
        {
        }

        /// <summary>
        /// Gets the application performance data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public IDictionary<string, IList<Models.AppPerformance>> GetAppPerformanceData(DateTime fromDate, DateTime toDate)
        {
            // TODO: possibly move this into a sproc? or improve the linq.
            var perfData = (from serviceApp in this.FindAll<ServiceApplication>()
                            join performanceItem in this.FindAll<ServiceApplicationPerfomance>(sap => sap.StartTime >= fromDate && sap.EndTime <= toDate)
                                on serviceApp.Id equals performanceItem.ServiceApplicationId
                            group performanceItem by serviceApp.Name into groupedData
                            select new
                            {
                                AppName = groupedData.Key,
                                Data = groupedData.Select(sap => new Models.AppPerformance
                                {
                                    Identifier = sap.ServiceApplicationId,
                                    StartTime = sap.StartTime,
                                    EndTime = sap.EndTime,
                                    Message = sap.Message
                                }).ToList()
                            }).AsNoTracking().ToList();

            return perfData.ToDictionary(keySelector: d => d.AppName, elementSelector: d => (IList<Models.AppPerformance>)d.Data);
        }
    }
}
