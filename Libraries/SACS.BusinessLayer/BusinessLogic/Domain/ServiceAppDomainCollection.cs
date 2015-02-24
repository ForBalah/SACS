using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// The collection of ServiceAppDomains
    /// </summary>
    public class ServiceAppDomainCollection : ICollection<ServiceAppDomain>
    {
        #region Fields

        private List<ServiceAppDomain> _collection;
        private IEqualityComparer<ServiceAppDomain> _comparer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppDomainCollection" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public ServiceAppDomainCollection(IEqualityComparer<ServiceAppDomain> comparer)
        {
            this._collection = new List<ServiceAppDomain>();
            this._comparer = comparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get { return this._collection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the <see cref="ServiceAppDomain"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="ServiceAppDomain"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ServiceAppDomain this[string name]
        {
            get
            {
                return this._collection.FirstOrDefault(d => d.ServiceApp != null && d.ServiceApp.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(ServiceAppDomain item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "ServiceAppDomain cannot be null");
            }

            if (item.ServiceApp == null)
            {
                throw new ArgumentNullException("item", "ServiceApp inside ServiceAppDomain cannot be null");
            }

            if (string.IsNullOrWhiteSpace(item.ServiceApp.Name))
            {
                throw new ArgumentNullException("item", "ServiceApp name cannot be null or empty");
            }

            if (this._collection.Contains(item, this._comparer))
            {
                throw new ArgumentException("ServiceAppDomain already exists in collection", "item");
            }

            this._collection.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            // make sure to unload because we are going to lose all references to the domains in this call.
            foreach (var domain in this._collection)
            {
                domain.Unload();
            }

            this._collection.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(ServiceAppDomain item)
        {
            return this._collection.Contains(item, this._comparer);
        }

        /// <summary>
        /// Copies to the array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(ServiceAppDomain[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0 || arrayIndex > this._collection.Count)
            {
                throw new IndexOutOfRangeException("arrayIndex");
            }

            int index = 0;

            while (index < array.Length && index + arrayIndex < this._collection.Count)
            {
                array[index] = this._collection[index + arrayIndex];
                index++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(ServiceAppDomain item)
        {
            if (this._collection.Contains(item))
            {
                this._collection.Remove(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ServiceAppDomain> GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._collection.GetEnumerator();
        } 

        #endregion
    }
}
