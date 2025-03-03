using CompanyManager.Logic.DataContext;

namespace CompanyManager.Logic.Contracts
{
    /// <summary>
    /// Represents a context that provides access to entity sets and supports saving changes.
    /// </summary>
    public interface IContext : IDisposable
    {
        /// <summary>
        /// Gets the entity set for companies.
        /// </summary>
        EntitySet<Entities.Company> CompanySet { get; }

        /// <summary>
        /// Gets the entity set for customers.
        /// </summary>
        EntitySet<Entities.Customer> CustomerSet { get; }

        /// <summary>
        /// Gets the entity set for employees.
        /// </summary>
        EntitySet<Entities.Employee> EmployeeSet { get; }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of state entries written to the underlying database.</returns>
        int SaveChanges();
    }
}