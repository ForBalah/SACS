using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SACS.Implementation.Utils
{
    /// <summary>
    /// The date time resolver class. Copied here for convenience. Be sure to use this
    /// instead of straight DateTime so that any class/method that uses this can be testable.
    /// </summary>
    internal class DateTimeResolver
    {
        private static Func<DateTime> _Resolve;

        public static Func<DateTime> Resolve
        {
            get
            {
                if (_Resolve == null)
                {
                    _Resolve = DefaultDateTime;
                }

                return _Resolve;
            }

            set
            {
                _Resolve = value;
            }
        }

        /// <summary>
        /// The default date time resolver - returns Now.
        /// </summary>
        /// <returns></returns>
        private static DateTime DefaultDateTime()
        {
            return DateTime.Now;
        }
    }
}