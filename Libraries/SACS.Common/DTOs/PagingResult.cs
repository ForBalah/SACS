using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Common.DTOs
{
    /// <summary>
    /// A DTO for holding a collection of items that have been paged
    /// </summary>
    /// <typeparam name="T">The class type contained in the list.</typeparam>
    public class PagingResult<T>
    {
        private IEnumerable<T> _Collection;
        private int _Total;
        private int _PageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResult{T}"/> class.
        /// </summary>
        public PagingResult()
        {
            this._Collection = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingResult{T}" /> class.
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
        public PagingResult(IEnumerable<T> collection, int total, int pageSize)
            : base()
        {
            if (collection != null && collection.Count() > total)
            {
                throw new ArgumentException("collection count cannot be greater than total");
            }

            this.Collection = collection;
            this.Total = total;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        /// <exception cref="System.ArgumentException">Total cannot be less than zero</exception>
        public int Total
        {
            get
            {
                return this._Total;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("total cannot be less than zero");
                }

                this._Total = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        /// <exception cref="System.ArgumentException">PageSize cannot be less than zero</exception>
        public int PageSize
        {
            get
            {
                return this._PageSize;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("pageSize cannot be less than zero");
                }

                this._PageSize = value;
            }
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        /// <exception cref="System.ArgumentNullException">Collection cannot be null</exception>
        public IEnumerable<T> Collection
        {
            get
            {
                return this._Collection;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("collection");
                }

                this._Collection = value;
            }
        }

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
