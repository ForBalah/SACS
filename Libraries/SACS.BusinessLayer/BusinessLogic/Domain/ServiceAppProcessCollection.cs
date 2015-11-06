using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// The collection of ServiceAppProcesss
    /// </summary>
    public class ServiceAppProcessCollection : ICollection<ServiceAppProcess>
    {
        #region Fields

        private List<ServiceAppProcess> _collection;
        private IEqualityComparer<ServiceAppProcess> _comparer;

        #endregion Fields

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAppProcessCollection" /> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public ServiceAppProcessCollection(IEqualityComparer<ServiceAppProcess> comparer)
        {
            this._collection = new List<ServiceAppProcess>();
            this._comparer = comparer;
        }

        #endregion Constructors and Destructors

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
        /// Gets the <see cref="ServiceAppProcess"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="ServiceAppProcess"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ServiceAppProcess this[string name]
        {
            get
            {
                return this._collection.FirstOrDefault(d => d.ServiceApp != null && d.ServiceApp.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(ServiceAppProcess item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "ServiceAppProcess cannot be null");
            }

            if (item.ServiceApp == null)
            {
                throw new ArgumentNullException("item", "ServiceApp inside ServiceAppProcess cannot be null");
            }

            if (string.IsNullOrWhiteSpace(item.ServiceApp.Name))
            {
                throw new ArgumentNullException("item", "ServiceApp name cannot be null or empty");
            }

            if (this._collection.Contains(item, this._comparer))
            {
                throw new ArgumentException("ServiceAppProcess already exists in collection", "item");
            }

            this._collection.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            // make sure to unload because we are going to lose all references to the processes in this call.
            foreach (var process in this._collection)
            {
                process.Stop();
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
        public bool Contains(ServiceAppProcess item)
        {
            return this._collection.Contains(item, this._comparer);
        }

        /// <summary>
        /// Copies to the array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(ServiceAppProcess[] array, int arrayIndex)
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
        public bool Remove(ServiceAppProcess item)
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
        public IEnumerator<ServiceAppProcess> GetEnumerator()
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

        #endregion Methods
    }
}