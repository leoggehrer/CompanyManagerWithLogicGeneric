using CompanyManager.Logic.Contracts;
using CompanyManager.Logic.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.WebApi.Contracts
{
    public interface IContextAccessor : IDisposable
    {
        IContext GetContext();
        DbSet<TEntity>? GetDbSet<TEntity>() where TEntity : class;
        EntitySet<TEntity>? GetEntitySet<TEntity>() where TEntity : Logic.Entities.EntityObject, new();
    }
}