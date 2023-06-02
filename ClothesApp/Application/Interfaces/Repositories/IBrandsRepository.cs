using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBrandsRepository : IBaseRepository<Brand>
{
    Task<bool> DoesBrandExist(string name);
    
    Task<Brand> DeleteBrand(Brand brand);
}