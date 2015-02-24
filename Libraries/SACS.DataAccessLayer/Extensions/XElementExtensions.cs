using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SACS.DataAccessLayer.Extensions
{
    /// <summary>
    /// The Extension methods for the XElement class
    /// </summary>
    internal static class XElementExtensions
    {
        /// <summary>
        /// Empties if null.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static XAttribute EmptyIfNull(this XAttribute attribute, string name)
        {
            return attribute ?? new XAttribute(name, string.Empty);
        }
    }
}
