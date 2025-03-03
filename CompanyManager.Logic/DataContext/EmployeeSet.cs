using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of Employee entities in the data context.
    /// </summary>
    internal sealed class EmployeeSet : EntitySet<Entities.Employee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The DbSet of Employee entities.</param>
        public EmployeeSet(DbContext context, DbSet<Entities.Employee> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source Employee entity to the target Employee entity.
        /// </summary>
        /// <param name="target">The target Employee entity.</param>
        /// <param name="source">The source Employee entity.</param>
        protected override void CopyProperties(Entities.Employee target, Entities.Employee source)
        {
            (target as Common.Contracts.IEmployee).CopyProperties(source);
        }
    }
}
