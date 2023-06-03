using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);
    
    Task<IList<Product>> FindByCondition(Expression<Func<Product, bool>> expression);
}