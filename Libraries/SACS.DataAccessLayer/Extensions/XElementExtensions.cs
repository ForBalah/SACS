using System.Xml.Linq;

namespace SACS.DataAccessLayer.Extensions
{
    /// <summary>
    /// The Extension methods for the XElement class
    /// </summary>
    internal static class XElementExtensions
    {
        /// <summary>
        /// Returns an empty attribute of the given name if the attribute is null
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
