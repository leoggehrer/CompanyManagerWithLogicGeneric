using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of entities that can be queried from a database and provides methods to manipulate them.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntitySet<TEntity> where TEntity : Entities.EntityObject, new()
    {
        #region fields
        private DbSet<TEntity> _set;
        #endregion fields

        #region properties
        /// <summary>
        /// Gets the database context.
        /// </summary>
        internal DbContext Context { get; private set; }

        /// <summary>
        /// Gets the queryable set of entities.
        /// </summary>
        public IQueryable<TEntity> QuerySet => _set.AsQueryable();
        #endregion properties

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySet{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The set of entities.</param>
        public EntitySet(DbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            _set = dbSet;
        }

        #region methods
        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <returns>A new instance of the entity.</returns>
        public TEntity Create()
        {
            return new TEntity();
        }

        /// <summary>
        /// Adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public TEntity Add(TEntity entity)
        {
            return _set.Add(entity).Entity;
        }

        /// <summary>
        /// Asynchronously adds the specified entity to the set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _set.AddAsync(entity).ConfigureAwait(false);
            return result.Entity;
        }

        /// <summary>
        /// Updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity, or null if the entity was not found.</returns>
        public TEntity? Update(int id, TEntity entity)
        {
            var existingEntity = _set.Find(id);
            if (existingEntity != null)
            {
                existingEntity.CopyProperties(entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Asynchronously updates the specified entity in the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to update.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity, or null if the entity was not found.</returns>
        public async Task<TEntity?> UpdateAsync(int id, TEntity entity)
        {
            var existingEntity = await _set.FindAsync(id).ConfigureAwait(false);
            if (existingEntity != null)
            {
                existingEntity.CopyProperties(entity);
            }
            return existingEntity;
        }

        /// <summary>
        /// Removes the entity with the specified identifier from the set.
        /// </summary>
        /// <param name="id">The identifier of the entity to remove.</param>
        /// <returns>The removed entity, or null if the entity was not found.</returns>
        public TEntity? Remove(int id)
        {
            var entity = _set.Find(id);
            if (entity != null)
            {
                _set.Remove(entity);
            }
            return entity;
        }
        #endregion methods
    }
}
