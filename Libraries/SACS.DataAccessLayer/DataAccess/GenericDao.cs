using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Entitites;

namespace SACS.DataAccessLayer.DataAccess
{
    /// <summary>
    /// Generic DAO for accessing logging database
    /// </summary>
    public class GenericDao : DaoBase, IGenericDao
    {
        #region Fields
        
        private SACSEntitiesContainer _dataContext;
        private IEnumerable<DbEntityValidationResult> _ValidationErrors;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="GenericDao"/> class from being created.
        /// </summary>
        protected GenericDao()
        {
            this._dataContext = new SACSEntitiesContainer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list on invalid entities.
        /// </summary>
        /// <value>The invalid entities.</value>
        public IList<DbEntityValidationResult> ValidationErrors
        {
            get
            {
                if (this._ValidationErrors == null)
                {
                    this._ValidationErrors = this._dataContext.GetValidationErrors();
                }

                return this._ValidationErrors.ToList();
            }
        }

        /// <summary>
        /// Gets the object context adapter. This is only used privately for some of the base objects needed.
        /// </summary>
        /// <value>The object context adapter.</value>
        private IObjectContextAdapter ObjectContextAdapter
        {
            get
            {
                if (this._dataContext != null)
                {
                    return (IObjectContextAdapter)this._dataContext;
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this._dataContext != null)
            {
                this._dataContext.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Loads a list of all the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> FindAll<TEntity>() where TEntity : class
        {
            return this.Repository<TEntity>();
        }

        /// <summary>
        /// Loads a list using a filter delegate
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IQueryable<TEntity> FindAll<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            if (filter == null)
            {
                return this.Repository<TEntity>();
            }
            else
            {
                return this.Repository<TEntity>().Where(filter);
            }
        }

        /// <summary>
        /// Gets the repository for the given type of entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>The repository of the given type.</returns>
        private IQueryable<TEntity> Repository<TEntity>() where TEntity : class
        {
            return this._dataContext.Set<TEntity>();
        }

        /// <summary>
        /// Adds the entity to the Database when SubmitChanges is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            this._dataContext.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Deletes the specified entity when SubmitChanges is called.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this._dataContext.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Determines whether this instance can submit changes.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can submit changes; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSubmitChanges()
        {
            // Get the current listing of invalid entities
            this._ValidationErrors = this._dataContext.GetValidationErrors();

            return this.ValidationErrors.Count == 0;
        }

        /// <summary>
        /// Updates a collection of objects in the object context with data from the data source.
        /// </summary>
        /// <param name="refreshMode">The refresh mode.</param>
        /// <param name="entity">The entity.</param>
        public void Refresh(RefreshMode refreshMode, object entity)
        {
            this.ObjectContextAdapter.ObjectContext.Refresh(refreshMode, entity);
        }

        /// <summary>
        /// Updates a collection of objects in the object context with data from the data source.
        /// </summary>
        /// <param name="refreshMode">The refresh mode.</param>
        /// <param name="collection">The collection.</param>
        public void Refresh(RefreshMode refreshMode, IEnumerable collection)
        {
            this.ObjectContextAdapter.ObjectContext.Refresh(refreshMode, collection);
        }

        /// <summary>
        /// Submits the changes.
        /// </summary>
        public void SubmitChanges()
        {
            if (this.CanSubmitChanges())
            {
                this._dataContext.SaveChanges();
            }
            else
            {
                throw new DbUpdateException(string.Format("Cannot submit changes on this Dao as it contains {0} invalid entities. Please check the InvalidEntities property for more details.", this.ValidationErrors.Count));
            }
        }

        #endregion
    }
}
