using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.BusinessLayer.BusinessLogic.Logs;
using SACS.BusinessLayer.Views;
using SACS.Common.Configuration;
using SACS.Common.DTOs;
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
        private static LogLoader logLoader = new LogLoader(LogLoader.DefaultDate);

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
            string location = ApplicationSettings.Current.AlternateLogLocation;
            bool showError = !string.IsNullOrWhiteSpace(location);
            PagingResult<LogEntry> entries = new PagingResult<LogEntry>();
            int pagingSize = ApplicationSettings.Current.DefaultPagingSize;

            this.TryExecute(
                () =>
                {
                    ILogsClient client = this.factory.Create<ILogsClient>();
                    entries = client.GetLogEntries(logFileName, page, searchQuery, pagingSize);
                },
                () =>
                {
                    if (!string.IsNullOrWhiteSpace(location))
                    {
                        var logs = new List<LogEntry>();
                        logLoader.LoadLogs(logs, Path.Combine(ApplicationSettings.Current.AlternateLogLocation, logFileName));
                        entries = LogLoader.FilterLogs(logs, page, searchQuery, pagingSize);
                    }
                },
                showError);

            return entries;
        }
    }
}