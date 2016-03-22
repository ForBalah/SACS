using SACS.DataAccessLayer.DataAccess.Interfaces;

namespace SACS.DataAccessLayer.Factories.Interfaces
{
    /// <summary>
    /// The abstract DAO factory.
    /// </summary>
    public interface IDaoFactory
    {
        /// <summary>
        /// Creates a new instance of the DAO, returning reference to it via its interface.
        /// </summary>
        /// <typeparam name="T">The DAO interface to resolve.</typeparam>
        /// <returns></returns>
        T Create<T>() where T : IDao;

        /// <summary>
        /// Creates a new instance of the DAO, returning reference to it via its interface.
        /// </summary>
        /// <typeparam name="T">The DAO interface to resolve.</typeparam>
        /// <param name="parameters">Parameters to pass into the factory's creation method.</param>
        /// <returns></returns>
        T Create<T>(params object[] parameters) where T : IDao;
    }
}
