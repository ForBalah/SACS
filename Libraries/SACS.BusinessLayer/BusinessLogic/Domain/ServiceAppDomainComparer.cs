using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.BusinessLayer.BusinessLogic.Domain
{
    /// <summary>
    /// Comparer used for ServiceAppDomains
    /// </summary>
    internal class ServiceAppDomainComparer : IEqualityComparer<ServiceAppDomain>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(ServiceAppDomain x, ServiceAppDomain y)
        {
            return x.ServiceApp != null && y.ServiceApp != null &&
                x.ServiceApp.Name.Equals(y.ServiceApp.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(ServiceAppDomain obj)
        {
            return obj.ServiceApp != null ? obj.ServiceApp.Name.GetHashCode() : obj.GetHashCode();
        }
    }
}
