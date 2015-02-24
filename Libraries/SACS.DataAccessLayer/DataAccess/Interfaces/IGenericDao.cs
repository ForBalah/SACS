using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SACS.DataAccessLayer.DataAccess.Interfaces
{
    /// <summary>
    /// Generic DAO interface
    /// </summary>
    public interface IGenericDao : IDisposable, IDao
    {
        #region Properties

        /// <summary>
        /// Gets a list on invalid entities.
        /// </summary>
        /// <value>The invalid entities.</value>
        IList<DbEntityValidationResult> ValidationErrors { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a list of all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> FindAll<TEntity>() where TEntity : class;

        /// <summary>
        /// Loads a list using a filter delegate
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        /// <summary>
        /// Adds the entity to the Database when SubmitChanges is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes the specified entity when SubmitChanges is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Determines whether this instance can submit changes.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can submit changes; otherwise, <c>false</c>.
        /// </returns>
        bool CanSubmitChanges();

        /// <summary>
        /// Updates a collection of objects in the object context with data from the data source.
        /// </summary>
        /// <param name="refreshMode">The refresh mode.</param>
        /// <param name="entity">The entity.</param>
        void Refresh(RefreshMode refreshMode, object entity);

        /// <summary>
        /// Updates a collection of objects in the object context with data from the data source.
        /// </summary>
        /// <param name="refreshMode">The refresh mode.</param>
        /// <param name="collection">The collection.</param>
        void Refresh(RefreshMode refreshMode, IEnumerable collection);

        /// <summary>
        /// Submits the changes.
        /// </summary>
        void SubmitChanges();

        #endregion
    }
}
