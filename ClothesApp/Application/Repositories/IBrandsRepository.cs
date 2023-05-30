using Domain.Entities;

namespace Application.Repositories;

public interface IBrandsRepository : IBaseRepository<Brand>
{
    Task<bool> DoesBrandExist(string name);

    Task<bool> DoesBrandExist(long id);

    Task<Brand> DeleteBrandById(long id);

    Task<Brand> GetBrandById(long id);
}