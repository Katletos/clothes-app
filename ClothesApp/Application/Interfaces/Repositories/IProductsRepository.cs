using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);

    Task<bool> DoesExist(string name);

    Task<IList<Product>> FindByCondition(Expression<Func<Product, bool>> expression);
}