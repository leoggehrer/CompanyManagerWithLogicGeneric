using CompanyManager.Logic.DataContext;
using CompanyManager.Common.Contracts;

namespace CompanyManager.Logic.Contracts
{
    public interface IContext : IDisposable
    {
        EntitySet<Entities.Company, ICompany> CompanySet { get; }
        EntitySet<Entities.Customer, ICustomer> CustomerSet { get; }
        EntitySet<Entities.Employee, IEmployee> EmployeeSet { get; }

        int SaveChanges();
    }
}