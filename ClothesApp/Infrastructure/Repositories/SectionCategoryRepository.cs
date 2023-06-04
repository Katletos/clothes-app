using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class SectionCategoryRepository : ISectionCategoryRepository
{
    private readonly ClothesAppContext _dbContext;

    public SectionCategoryRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<SectionCategory> Insert(SectionCategory entity)
    {
        throw new NotImplementedException();
    }

    public Task<SectionCategory> Update(SectionCategory entity)
    {
        throw new NotImplementedException();
    }

    public Task<IList<SectionCategory>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<SectionCategory> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesExist(long id)
    {
        throw new NotImplementedException();
    }
}