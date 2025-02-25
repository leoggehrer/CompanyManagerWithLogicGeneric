using CompanyManager.Logic.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CustomerSet : EntitySet<Entities.Customer>
    {
        public CustomerSet(DbContext context, DbSet<Customer> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Customer target, Customer source)
        {
            (target as Common.Contracts.ICustomer).CopyProperties(source);
        }
    }
}
