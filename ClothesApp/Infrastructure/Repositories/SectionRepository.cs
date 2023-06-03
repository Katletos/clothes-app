using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly ClothesAppContext _dbContext;

    public SectionRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }
    public async Task<Section> Insert(Section section)
    {
        _dbContext.Sections.Add(section);
        await _dbContext.SaveChangesAsync();

        return section;
    }

    public async Task<Section> Update(Section section)
    {
        _dbContext.Update(section);
        await _dbContext.SaveChangesAsync();

        return section;
    }

    public async Task<IList<Section>> GetAll()
    {
        return await _dbContext.Sections.AsNoTracking().ToListAsync();
    }

    public async Task<Section> GetById(long id)
    {
        return await _dbContext.Sections.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Sections.AnyAsync(s => s.Id == id);
    }
}