using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.Models;

namespace SACS.BusinessLayer.BusinessLogic.Logs
{
    /// <summary>
    /// The search criteria for logs
    /// </summary>
    public class LogSearchCriteria
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PagingSize { get; set; }

        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Filters the logs.
        /// </summary>
        /// <param name="logs">The logs.</param>
        /// <returns>
        /// The logs filtered based on the search criteria
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// PagingSize cannot be less than zero
        /// or
        /// PageNumber cannot be less than zero.
        /// </exception>
        public virtual IList<LogEntry> FilterLogs(IList<LogEntry> logs)
        {
            if (this.PagingSize < 0)
            {
                throw new InvalidOperationException("PagingSize cannot be less than zero");
            }

            if (this.PageNumber < 0)
            {
                throw new InvalidOperationException("PageNumber cannot be less than zero");
            }

            IEnumerable<LogEntry> filteredLogs = logs;
            if (!string.IsNullOrWhiteSpace(this.SearchQuery))
            {
                filteredLogs = filteredLogs.Where(l => l.ContainsText(this.SearchQuery));
            }

            // TODO: all this needs to be moved into the paging result - the criteria shouldn't care about
            // paging because now this is confusing and no longer SRP
            this.Total = filteredLogs.Count();
            if (this.PagingSize > 0)
            {
                int skipCount = this.PageNumber * this.PagingSize;

                if (skipCount > filteredLogs.Count())
                {
                    filteredLogs = new List<LogEntry>();
                }
                else
                {
                    filteredLogs = filteredLogs.Skip(this.PageNumber * this.PagingSize).Take(this.PagingSize);
                }
            }

            return filteredLogs.ToList();
        }

        #endregion
    }
}
