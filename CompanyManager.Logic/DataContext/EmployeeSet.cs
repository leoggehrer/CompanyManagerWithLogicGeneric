using CompanyManager.Logic.Entities;
using CompanyManager.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal sealed partial class EmployeeSet : EntitySet<Employee, IEmployee>
    {
        public EmployeeSet(DbContext context, DbSet<Employee> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Employee target, IEmployee source)
        {
            (target as IEmployee).CopyProperties(source);
        }
    }
}
