using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// Presenter for the Analytics view
    /// </summary>
    public class AnalyticsPresenter : PresenterBase<IAnalyticsView>
    {
        private readonly IRestClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public AnalyticsPresenter(IAnalyticsView view, IRestClientFactory factory)
            : base(view)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public void LoadData(DateTime fromDate, DateTime toDate)
        {
            this.TryExecute(() =>
            {
                IAnalyticsClient client = this.factory.Create<IAnalyticsClient>();
                this.View.SetPerformanceData(client.GetAllAppPerformances(fromDate, toDate));
            },null,
            true);
        }
    }
}
