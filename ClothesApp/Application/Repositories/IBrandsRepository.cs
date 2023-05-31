using Domain.Entities;

namespace Application.Repositories;

public interface IBrandsRepository : IBaseRepository<Brand>
{
    Task<bool> DoesBrandExist(string name);

    Task<bool> DoesBrandExist(long id);

    Task<Brand> DeleteBrand(Brand brand);
}