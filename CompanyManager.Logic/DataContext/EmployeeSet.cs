using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class EmployeeSet : EntitySet<Entities.Employee>
    {
        public EmployeeSet(DbContext context, DbSet<Entities.Employee> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Entities.Employee target, Entities.Employee source)
        {
            (target as Common.Contracts.IEmployee).CopyProperties(source);
        }
    }
}
