using CompanyManager.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CompanyContext : DbContext, IContext
    {
        #region fields
        private static string ConnectionString = "data source=CompanyManager.db";
        #endregion fields

        #region properties
        internal DbSet<Entities.Company> DbCompanySet { get; set; }
        internal DbSet<Entities.Customer> DbCustomerSet { get; set; }
        internal DbSet<Entities.Employee> DbEmployeeSet { get; set; }

        public EntitySet<Entities.Company> CompanySet { get; }
        public EntitySet<Entities.Customer> CustomerSet { get; }
        public EntitySet<Entities.Employee> EmployeeSet { get; }
        #endregion properties

        #region constructors
        public CompanyContext()
        {
            CompanySet = new EntitySet<Entities.Company>(this, DbCompanySet!);
            CustomerSet = new EntitySet<Entities.Customer>(this, DbCustomerSet!);
            EmployeeSet = new EntitySet<Entities.Employee>(this, DbEmployeeSet!);
        }
        #endregion constructors

        #region methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
        #endregion methods
    }
}
