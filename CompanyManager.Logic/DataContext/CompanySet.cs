using CompanyManager.Logic.Entities;
using CompanyManager.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal sealed partial class CompanySet : EntitySet<Company, ICompany>
    {
        public CompanySet(DbContext context, DbSet<Company> dbSet) : base(context, dbSet)
        {
        }

        protected override void CopyProperties(Company target, ICompany source)
        {
            (target as ICompany).CopyProperties(source);
        }
    }
}
