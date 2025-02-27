using CompanyManager.Logic.Entities;
using CompanyManager.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal sealed partial class CustomerSet : EntitySet<Customer, ICustomer>
    {
        public CustomerSet(DbContext context, DbSet<Customer> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Customer target, ICustomer source)
        {
            (target as ICustomer).CopyProperties(source);
        }
    }
}
