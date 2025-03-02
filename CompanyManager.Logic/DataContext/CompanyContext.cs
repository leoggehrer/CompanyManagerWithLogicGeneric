using CompanyManager.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    internal class CompanyContext : DbContext, IContext
    {
        #region fields
        private static string DatabaseType = "Sqlite";
        private static string ConnectionString = "data source=CompanyManager.db";
        #endregion fields

        static CompanyContext()
        {
            var appSettings = Common.Modules.Configuration.AppSettings.Instance;

            DatabaseType = appSettings["Database:Type"] ?? DatabaseType;
            ConnectionString = appSettings[$"ConnectionStrings:{DatabaseType}ConnectionString"] ?? ConnectionString;
        }

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
            CompanySet = new CompanySet(this, DbCompanySet!);
            CustomerSet = new CustomerSet(this, DbCustomerSet!);
            EmployeeSet = new EmployeeSet(this, DbEmployeeSet!);
        }
        #endregion constructors

        #region methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (DatabaseType == "Sqlite")
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
            else if (DatabaseType == "SqlServer")
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
        #endregion methods
    }
}
