using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CompanySet : EntitySet<Entities.Company>
    {
        public CompanySet(DbContext context, DbSet<Entities.Company> dbSet) : base(context, dbSet)
        {
        }

        protected override void CopyProperties(Entities.Company target, Entities.Company source)
        {
            (target as Common.Contracts.ICompany).CopyProperties(source);
        }
    }
}
