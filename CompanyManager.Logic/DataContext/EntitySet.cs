using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of entities that can be queried from a database and provides methods to manipulate them.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TContract">The interface that implements the entity.</typeparam>
    public abstract partial class EntitySet<TEntity, TContract> 
        where TContract : Common.Contracts.IIdentifiable
        where TEntity : Entities.EntityObject, TContract, new()
    {
        #region fields
        private DbSet<TEntity> _dbSet;
        #endregion fields

        #region properties
        /// <summary>
        /// Gets the database context.
        /// </summary>
        internal DbContext Context { get; private set; }

        /// <summary>
        /// Gets the queryable set of entities.
        /// </summary>
        public IQueryable<TContract> QuerySet => _dbSet.AsQueryable<TContract>();
        #endregion properties

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySet{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The set of entities.</param>
        public EntitySet(DbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            _dbSet = dbSet;
        }

        #region methods
        protected abstract void CopyProperties(TEntity target, TContract source);

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <returns>A new instance of the entity.</returns>
        public virtual TContract Create()
        {
            return new TEntity();
        }

        public virtual TContract Create(TContract other)
        {
            var result = new TEntity();

            CopyProperties(result, other);
            return result;
        }
        /// <summary>
        /// Adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public virtual TContract Add(TContract entity)
        {
            var newEntity = new TEntity();

            CopyProperties(newEntity, entity);
            return _dbSet.Add(newEntity).Entity;
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        public virtual async Task<TContract> AddAsync(TEntity entity)
        {
            var newEntity = new TEntity();

            CopyProperties(newEntity, entity);

            var result = await _dbSet.AddAsync(newEntity).ConfigureAwait(false);

            return result.Entity;
        }

        /// <summary>
        /// Updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity, or null if the entity was not found.</returns>
        public virtual TContract? Update(int id, TContract entity)
        {
            var existingEntity = _dbSet.Find(id);
            if (existingEntity != null)
            {
                CopyProperties(existingEntity, entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Asynchronously updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity, or null if the entity was not found.</returns>
        public virtual async Task<TContract?> UpdateAsync(int id, TContract entity)
        {
            var existingEntity = await _dbSet.FindAsync(id).ConfigureAwait(false);
            if (existingEntity != null)
            {
                CopyProperties(existingEntity, entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Removes the entity with the specified identifier from the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to remove.</param>
        /// <returns>The removed entity, or null if the entity was not found.</returns>
        public virtual TContract? Remove(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            return entity;
        }
        #endregion methods
    }
}
