using Domain.Entities;

namespace Application.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<bool> AnyProductOfBrandIdExists(long id);
}