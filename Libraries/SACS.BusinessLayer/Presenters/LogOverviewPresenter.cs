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
    /// The presenter for the Logs view
    /// </summary>
    public class LogOverviewPresenter : PresenterBase<ILogOverviewView>
    {
        private readonly IRestClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SACS.BusinessLayer.Presenters.LogOverviewPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="factory">The REST client factory.</param>
        public LogOverviewPresenter(ILogOverviewView view, IRestClientFactory factory)
            : base(view)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        public void LoadControl()
        {
            var logPaths = this.GetLogPaths();
            this.View.SetCurrentLogs(logPaths);
        }

        /// <summary>
        /// Gets the log paths.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetLogPaths()
        {
            IEnumerable<string> logs = new List<string>();
            this.TryExecute(
                () =>
                {
                    ILogsClient client = this.factory.Create<ILogsClient>();
                    logs = client.GetLogNames();
                    this.View.ClearException();
                },
                null,
                true);

            return logs;
        }
    }
}
