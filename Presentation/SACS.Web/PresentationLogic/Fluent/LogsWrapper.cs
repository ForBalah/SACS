using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SACS.BusinessLayer.Presenters;
using SACS.BusinessLayer.Views;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.DataAccessLayer.Models;

namespace SACS.Web.PresentationLogic.Fluent
{
    /// <summary>
    /// Fluent class wrapping the logs MVP functionality
    /// </summary>
    public class LogsWrapper
    {
        private readonly LogOverviewPresenter _overviewPresenter;
        private readonly LogDetailPresenter _detailPresenter;
        private readonly LogOverviewView _overviewView = new LogOverviewView();
        private readonly LogDetailView _detailView = new LogDetailView();

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsWrapper"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public LogsWrapper(IRestClientFactory factory)
        {
            this._overviewPresenter = new LogOverviewPresenter(this._overviewView, factory);
            this._detailPresenter = new LogDetailPresenter(this._detailView, factory);
        }

        /// <summary>
        /// Gets the logic exception.
        /// </summary>
        /// <value>
        /// The logic exception.
        /// </value>
        public Tuple<string, Exception> ExceptionItem
        {
            get
            {
                return this._overviewView.Exception ?? this._detailView.Exception;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return this.ExceptionItem == null;
            }
        }

        /// <summary>
        /// Gets the log list.
        /// </summary>
        /// <value>
        /// The log list.
        /// </value>
        public IEnumerable<string> LogList
        {
            get
            {
                return this._overviewView.LogList;
            }
        }

        /// <summary>
        /// Gets the log entries.
        /// </summary>
        /// <value>
        /// The log entries.
        /// </value>
        public IList<LogEntry> LogEntries
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads the log entries into the wrapper.
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        /// <param name="page">The page.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public LogsWrapper LoadLogEntries(string logFileName, int page, string query)
        {
            this._overviewView.ClearException();
            this._detailView.Clear();
            this.LogEntries = this._detailPresenter.GetEntries(logFileName, page, query);
            return this;
        }

        /// <summary>
        /// Loads the log list.
        /// </summary>
        /// <returns></returns>
        public LogsWrapper LoadLogList()
        {
            this._overviewView.ClearException();
            this._detailView.Clear();
            this._overviewPresenter.LoadControl();
            return this;
        }
    }

    /// <summary>
    /// Implementation of ILogOverviewView
    /// </summary>
    internal class LogOverviewView : ILogOverviewView
    {
        /// <summary>
        /// Gets the logic exception.
        /// </summary>
        /// <value>
        /// The logic exception.
        /// </value>
        public Tuple<string, Exception> Exception { get; private set; }

        /// <summary>
        /// Gets the log list.
        /// </summary>
        /// <value>
        /// The log list.
        /// </value>
        public IEnumerable<string> LogList { get; private set; }

        /// <summary>
        /// Sets the current logs.
        /// </summary>
        /// <param name="logs">The logs.</param>
        public void SetCurrentLogs(IEnumerable<string> logs)
        {
            this.LogList = logs;
        }

        /// <summary>
        /// Clears the exception.
        /// </summary>
        public void ClearException()
        {
            this.Exception = null;
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            this.Exception = new Tuple<string, Exception>(title, e);
        }
    }

    /// <summary>
    /// Implementation of ILogDetailView
    /// </summary>
    internal class LogDetailView : ILogDetailView
    {
        /// <summary>
        /// Gets the logic exception.
        /// </summary>
        /// <value>
        /// The logic exception.
        /// </value>
        public Tuple<string, Exception> Exception { get; private set; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Exception = null;
        }

        /// <summary>
        /// Shows the exception generated.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="e">The exception.</param>
        public void ShowException(string title, Exception e)
        {
            this.Exception = new Tuple<string, Exception>(title, e);
        }
    }
}