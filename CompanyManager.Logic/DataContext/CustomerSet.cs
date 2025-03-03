using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of customer entities in the data context.
    /// </summary>
    internal sealed class CustomerSet : EntitySet<Entities.Customer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerSet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The DbSet of customer entities.</param>
        public CustomerSet(DbContext context, DbSet<Entities.Customer> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source customer entity to the target customer entity.
        /// </summary>
        /// <param name="target">The target customer entity.</param>
        /// <param name="source">The source customer entity.</param>
        protected override void CopyProperties(Entities.Customer target, Entities.Customer source)
        {
            (target as Common.Contracts.ICustomer).CopyProperties(source);
        }
    }
}
