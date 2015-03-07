using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.Structs
{
    /// <summary>
    /// A struct for holding collection items
    /// </summary>
    public struct PagingResult<T>
    {
        private IEnumerable<T> _Collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResult{T}" /> struct.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="total">The total in the expanded collection, independent of the collection count.</param>
        /// <param name="pageSize">Size of the page, zero means no page size.</param>
        /// <exception cref="System.ArgumentException">
        /// Total cannot be less than zero
        /// or
        /// pageSize cannot be less than zero
        /// or
        /// collection count cannot be greater than total.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">collection</exception>
        public PagingResult(IEnumerable<T> collection, int total, int pageSize) : this()
        {
            if (total < 0)
            {
                throw new ArgumentException("total cannot be less than zero");
            }

            if (pageSize < 0)
            {
                throw new ArgumentException("pageSize cannot be less than zero");
            }

            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (collection.Count() > total)
            {
                throw new ArgumentException("collection count cannot be greater than total");
            }

            this.Collection = collection;
            this.Total = total;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the total count of items.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IEnumerable<T> Collection
        {
            get
            {
                this._Collection = this._Collection ?? new List<T>();
                return this._Collection;
            }

            private set
            {
                this._Collection = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the number of pages.
        /// </summary>
        /// <value>
        /// The number of pages.
        /// </value>
        public int NumberOfPages
        {
            get
            {
                if (this.PageSize == 0)
                {
                    return this.Total > 0 ? 1 : 0;
                }

                if (this.Total == 0)
                {
                    return 0;
                }

                return ((this.Total - 1) / this.PageSize) + 1;
            }
        }
    }
}
