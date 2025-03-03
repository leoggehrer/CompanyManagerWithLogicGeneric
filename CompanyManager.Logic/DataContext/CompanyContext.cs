using CompanyManager.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.DataContext
{
    /// <summary>
    /// Represents the database context for the CompanyManager application.
    /// </summary>
    internal class CompanyContext : DbContext, IContext
    {
        #region fields
        /// <summary>
        /// The type of the database (e.g., "Sqlite", "SqlServer").
        /// </summary>
        private static string DatabaseType = "Sqlite";

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private static string ConnectionString = "data source=CompanyManager.db";
        #endregion fields

        /// <summary>
        /// Initializes static members of the <see cref="CompanyContext"/> class.
        /// </summary>
        static CompanyContext()
        {
            var appSettings = Common.Modules.Configuration.AppSettings.Instance;

            DatabaseType = appSettings["Database:Type"] ?? DatabaseType;
            ConnectionString = appSettings[$"ConnectionStrings:{DatabaseType}ConnectionString"] ?? ConnectionString;
        }

        #region properties
        /// <summary>
        /// Gets or sets the DbSet for Company entities.
        /// </summary>
        internal DbSet<Entities.Company> DbSetCompany { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Customer entities.
        /// </summary>
        internal DbSet<Entities.Customer> DbSetCustomer { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for Employee entities.
        /// </summary>
        internal DbSet<Entities.Employee> DbSetEmployee { get; set; }

        /// <summary>
        /// Gets the EntitySet for Company entities.
        /// </summary>
        public EntitySet<Entities.Company> CompanySet { get; }

        /// <summary>
        /// Gets the EntitySet for Customer entities.
        /// </summary>
        public EntitySet<Entities.Customer> CustomerSet { get; }

        /// <summary>
        /// Gets the EntitySet for Employee entities.
        /// </summary>
        public EntitySet<Entities.Employee> EmployeeSet { get; }
        #endregion properties

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyContext"/> class.
        /// </summary>
        public CompanyContext()
        {
            CompanySet = new CompanySet(this, DbSetCompany!);
            CustomerSet = new CustomerSet(this, DbSetCustomer!);
            EmployeeSet = new EmployeeSet(this, DbSetEmployee!);
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// Configures the database context options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to be used for configuration.</param>
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
