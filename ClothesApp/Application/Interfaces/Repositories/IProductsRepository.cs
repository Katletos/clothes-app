using System.Linq.Expressions;
using Application.Dtos.OrderItems;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);

    Task<bool> DoesExist(string name);

    Task<IList<Product>> FindByCondition(Expression<Func<Product, bool>> expression);

    Task UpdateRange(IList<Product> products);

    Task<bool> DoesExistRange(IList<long> ids);

    Task<IList<Product>> GetRange(IList<long> ids);
}