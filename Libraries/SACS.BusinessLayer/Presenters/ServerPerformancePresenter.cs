using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// The Presenter for the ServerPerformance view
    /// </summary>
    public class ServerPerformancePresenter : PresenterBase<IServerPerformanceView>
    {
        private readonly IRestClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerPerformancePresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="factory">The REST client factory.</param>
        public ServerPerformancePresenter(IServerPerformanceView view, IRestClientFactory factory)
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
            this.TryExecute(
                () =>
                {
                    IAnalyticsClient client = this.factory.Create<IAnalyticsClient>();
                    this.View.SetCpuPerformanceData(client.GetSystemCpuPerformance(fromDate, toDate));
                },
                null,
                true);

            this.TryExecute(
                () =>
                {
                    IAnalyticsClient client = this.factory.Create<IAnalyticsClient>();
                    this.View.SetMemoryPerformanceData(client.GetSystemMemoryPerformance(fromDate, toDate));
                },
                null,
                true);
        }
    }
}
