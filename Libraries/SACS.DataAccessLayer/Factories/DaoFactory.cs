using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;

namespace SACS.DataAccessLayer.Factories
{
    /// <summary>
    /// The DAO factory that ensures that only interfaces are returned.
    /// </summary>
    public class DaoFactory
    {
        /// <summary>
        /// Creates the DAO, passing it back as an interface. It will chain up until it finds a DAO constructor.
        /// </summary>
        /// <typeparam name="T">The DAO interface to return.</typeparam>
        /// <typeparam name="TDao">The type of the DAO.</typeparam>
        /// <returns></returns>
        public static T Create<T, TDao>()
            where T : IDao
            where TDao : DaoBase
        {
            return Create<T, TDao>(null);
        }

        /// <summary>
        /// Creates the DAO with a constructor that accepts the provided parameters, passing it back as an interface
        /// </summary>
        /// <typeparam name="T">The DAO interface to return.</typeparam>
        /// <typeparam name="TDao">The type of the DAO wrapped by the interface.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static T Create<T, TDao>(params object[] parameters)
            where T : IDao
            where TDao : DaoBase
        {
            return (T)Activator.CreateInstance(
                        typeof(TDao),
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        default(Binder),
                        parameters,
                        CultureInfo.InvariantCulture);
        }
    }
}
