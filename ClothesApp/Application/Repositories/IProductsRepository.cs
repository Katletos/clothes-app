using Domain.Entities;

namespace Application.Repositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);
}