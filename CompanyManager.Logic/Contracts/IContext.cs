using CompanyManager.Logic.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Logic.Contracts
{
    public interface IContext : IDisposable
    {
        EntitySet<Entities.Company> CompanySet { get; }
        EntitySet<Entities.Customer> CustomerSet { get; }
        EntitySet<Entities.Employee> EmployeeSet { get; }

        int SaveChanges();
    }
}