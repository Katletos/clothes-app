using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IBrandsRepository : IBaseRepository<Brand>
{
    Task<bool> DoesExist(string name);

    Task<Brand> Delete(Brand brand);
}