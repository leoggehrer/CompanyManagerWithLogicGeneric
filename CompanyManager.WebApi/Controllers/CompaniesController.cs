using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CompanyManager.WebApi.Controllers
{
    using TModel = Models.Company;
    using TEntity = Logic.Entities.Company;
    using TContract = Common.Contracts.ICompany;

    /// <summary>
    /// Controller for managing companies.
    /// </summary>
    public class CompaniesController : GenericController<TModel, TEntity, TContract>
    {
        /// <summary>
        /// Gets the query set for the entity.
        /// </summary>
        protected override IQueryable<TEntity> QuerySet
        {
            get
            {
                var result = default(IQueryable<TEntity>);

                // If the action is 'GetById(...)', then include the customers in the query.
                if (ControllerContext.ActionDescriptor.ActionName == nameof(GetById))
                {
                    result = EntitySet.Include(e => e.Customers).AsQueryable();
                }
                else
                {
                    result = EntitySet.AsQueryable();
                }
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompaniesController"/> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public CompaniesController(Contracts.IContextAccessor contextAccessor)
            : base(contextAccessor)
        {
        }

        /// <summary>
        /// Converts an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The model.</returns>
        protected override TModel ToModel(TEntity entity)
        {
            var result = new TModel();

            (result as TContract).CopyProperties(entity);
            if (entity.Customers != null)
            {
                result.Customers = entity.Customers.Select(e =>
                {
                    var result = new Models.Customer();

                    (result as Common.Contracts.ICustomer).CopyProperties(e);
                    return result;
                }).ToArray();
            }
            return result;
        }

        /// <summary>
        /// Converts a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        protected override TEntity ToEntity(TModel model, TEntity? entity)
        {
            var result = entity ??= new TEntity();

            (result as TContract).CopyProperties(model);
            return result;
        }
    }
}
