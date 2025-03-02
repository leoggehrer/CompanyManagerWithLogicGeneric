using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CustomerSet : EntitySet<Entities.Customer>
    {
        public CustomerSet(DbContext context, DbSet<Entities.Customer> dbSet) : base(context, dbSet)
        {
        }
        protected override void CopyProperties(Entities.Customer target, Entities.Customer source)
        {
            (target as Common.Contracts.ICustomer).CopyProperties(source);
        }
    }
}
