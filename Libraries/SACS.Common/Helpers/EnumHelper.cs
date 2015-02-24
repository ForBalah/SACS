using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Common.Enums;

namespace SACS.Common.Helpers
{
    /// <summary>
    /// Enum helper methods
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Parses the specified value into the Enum type
        /// </summary>
        /// <typeparam name="T">The Enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T Parse<T>(string value) where T : struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="theEnum">The enum.</param>
        /// <returns></returns>
        public static string GetName(this Enum theEnum)
        {
            return Enum.GetName(theEnum.GetType(), theEnum);
        }

        /// <summary>
        /// Gets the enum list.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <returns></returns>
        public static IList<EnumWrapper> GetEnumList<TEnum>() where TEnum : struct, IConvertible
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(e => new EnumWrapper(e as Enum))
                .ToList();
        }
    }
}
