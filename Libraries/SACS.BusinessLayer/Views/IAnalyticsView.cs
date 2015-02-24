using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.Views
{
    /// <summary>
    /// The application analytics view
    /// </summary>
    public interface IAnalyticsView : IViewBase
    {
        /// <summary>
        /// Sets the performance data.
        /// </summary>
        /// <param name="data">The data.</param>
        void SetPerformanceData(IDictionary<string, IList<AppPerformance>> data);
    }
}
