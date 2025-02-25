using CompanyManager.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CompanySet : EntitySet<Entities.Company>
    {
        public CompanySet(DbContext context, DbSet<Company> dbSet) : base(context, dbSet)
        {
        }

        protected override void CopyProperties(Company target, Company source)
        {
            (target as Common.Contracts.ICompany).CopyProperties(source);
        }
    }
}
