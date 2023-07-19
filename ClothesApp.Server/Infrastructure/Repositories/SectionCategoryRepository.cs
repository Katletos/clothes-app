using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SectionCategoryRepository : ISectionCategoryRepository
{
    private readonly ClothesAppContext _dbContext;

    public SectionCategoryRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> DoesExist(long sectionId, long categoryId)
    {
        return await _dbContext.SectionCategories.AnyAsync(sc => sc.CategoryId == categoryId
                                                                 && sc.SectionId == sectionId);
    }

    public async Task<bool> DoesSectionRelateCategory(long id)
    {
        return await _dbContext.SectionCategories.AnyAsync(sc => sc.SectionId == id);
    }

    public async Task<SectionCategory> Add(SectionCategory sectionCategory)
    {
        _dbContext.SectionCategories.Add(sectionCategory);
        await _dbContext.SaveChangesAsync();

        return sectionCategory;
    }
}