using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;
using SACS.DataAccessLayer.WebAPI.Interfaces;

namespace SACS.BusinessLayer.Presenters
{
    /// <summary>
    /// The log detail presenter
    /// </summary>
    public class LogDetailPresenter : PresenterBase<ILogDetailView>
    {
        private readonly IRestClientFactory factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public LogDetailPresenter(ILogDetailView view, IRestClientFactory factory)
            : base(view)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        /// <returns></returns>
        public IList<LogEntry> GetEntries(string logFileName)
        {
            IList<LogEntry> entries = new List<LogEntry>();

            this.TryExecute(() =>
            {
                ILogsClient client = this.factory.Create<ILogsClient>();
                entries = client.GetLogEntries(logFileName);
            }, null,
            true);

            return entries;
        }
    }
}
