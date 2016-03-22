using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories.Interfaces;

namespace SACS.DataAccessLayer.Factories
{
    /// <summary>
    /// The default DAO factory.
    /// </summary>
    public class DaoFactory : IDaoFactory
    {
        private Dictionary<Type, Type> _daoMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaoFactory"/> class.
        /// </summary>
        public DaoFactory()
        {
            _daoMapping = new Dictionary<Type, Type>();
        }

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
            // TODO: remove
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
            // TODO: remove
            return (T)Activator.CreateInstance(
                        typeof(TDao),
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        default(Binder),
                        parameters,
                        CultureInfo.InvariantCulture);
        }

        public T Create<T>() where T : IDao
        {
            return Create<T>(null);
        }

        public T Create<T>(params object[] parameters) where T : IDao
        {
            return (T)Activator.CreateInstance(
                _daoMapping[typeof(T)],
                BindingFlags.NonPublic | BindingFlags.Instance,
                default(Binder),
                parameters,
                CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Registers the interface type with the implementation type.
        /// </summary>
        /// <typeparam name="T">The DAO interface to register against.</typeparam>
        /// <typeparam name="TDao">The implementation to register.</typeparam>
        public void RegisterDao<T, TDao>()
            where T : IDao
            where TDao : DaoBase
        {
            // Work in progress. Eventually this should go into the IoC container but I
            // needed a quick fix.
            _daoMapping.Add(typeof(T), typeof(TDao));
        }
    }
}
