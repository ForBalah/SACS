using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// The Server performance view
    /// </summary>
    public interface IServerPerformanceView : IViewBase
    {
        /// <summary>
        /// Sets the cpu performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        void SetCpuPerformanceData(IList<SystemPerformance> data);

        /// <summary>
        /// Sets the memory performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        void SetMemoryPerformanceData(IList<SystemPerformance> data);
    }
}
