using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProductsRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);
}