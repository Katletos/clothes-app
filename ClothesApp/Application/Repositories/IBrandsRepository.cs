using Domain.Entities;

namespace Application.Repositories;

public interface IBrandsRepository : IBaseRepository<Brand>
{
    Task<bool> DoesBrandExistAsync(string name);

    Task<bool> DoesBrandExistAsync(long id);

    Task<Brand> DeleteBrandByIdAsync(long id);

    Task<Brand> GetBrandByIdAsync(long id);
}