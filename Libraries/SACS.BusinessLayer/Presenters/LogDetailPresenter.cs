using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.Views;
using SACS.Common.Structs;
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
        /// Initializes a new instance of the <see cref="LogDetailPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="factory">The REST client factory.</param>
        public LogDetailPresenter(ILogDetailView view, IRestClientFactory factory)
            : base(view)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        /// <param name="page">The page.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        public PagingResult<LogEntry> GetEntries(string logFileName, int? page = null, string searchQuery = null)
        {
            PagingResult<LogEntry> entries = new PagingResult<LogEntry>();

            this.TryExecute(
                () =>
                {
                    ILogsClient client = this.factory.Create<ILogsClient>();
                    entries = client.GetLogEntries(logFileName, page, searchQuery);
                },
                null,
                true);

            return entries;
        }
    }
}
