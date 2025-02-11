using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    public class EntitySet<TEntity> where TEntity : Entities.EntityObject, new()
    {
        #region fields
        private DbSet<TEntity> _set;
        #endregion fields

        #region properties
        internal DbContext Context { get; private set; }
        public IQueryable<TEntity> QuerySet => _set.AsQueryable();
        #endregion properties

        public EntitySet(DbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            _set = dbSet;
        }

        #region methods
        public TEntity Create()
        {
            return new TEntity();
        }
        public TEntity Add(TEntity entity)
        {
            return _set.Add(entity).Entity;
        }
        public TEntity? Update(int id, TEntity entity)
        {
            var existingEntity = _set.Find(id);

            if (existingEntity != null)
            {
                existingEntity.CopyProperties(entity);
            }
            return existingEntity;
        }
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
