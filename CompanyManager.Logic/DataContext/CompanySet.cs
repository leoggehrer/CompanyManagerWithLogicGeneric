using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents a set of <see cref="Entities.Company"/> entities in the data context.
    /// </summary>
    internal sealed class CompanySet : EntitySet<Entities.Company>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanySet"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="dbSet">The database set of <see cref="Entities.Company"/> entities.</param>
        public CompanySet(DbContext context, DbSet<Entities.Company> dbSet) : base(context, dbSet)
        {
        }

        /// <summary>
        /// Copies the properties from the source <see cref="Entities.Company"/> to the target <see cref="Entities.Company"/>.
        /// </summary>
        /// <param name="target">The target <see cref="Entities.Company"/>.</param>
        /// <param name="source">The source <see cref="Entities.Company"/>.</param>
        protected override void CopyProperties(Entities.Company target, Entities.Company source)
        {
            (target as Common.Contracts.ICompany).CopyProperties(source);
        }
    }
}
