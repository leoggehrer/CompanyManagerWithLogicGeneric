using CompanyManager.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class EmployeeSet : EntitySet<Entities.Employee>
    {
        public EmployeeSet(DbContext context, DbSet<Employee> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Employee target, Employee source)
        {
            (target as Common.Contracts.IEmployee).CopyProperties(source);
        }
    }
}
