using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ClothesAppContext _dbContext;

    public CategoryRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }

    public async Task<Category> Insert(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<Category> Update(Category category)
    {
        _dbContext.Update(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<IList<Category>> GetAll()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category> GetById(long id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<IList<Category>> FindByCondition(Expression<Func<Category, bool>> expression)
    {
        return await _dbContext.Categories.Where(expression).ToListAsync();
    }

    public async Task<bool> AreSameName(long id, string categoryName)
    {
        return await _dbContext.Categories.AnyAsync(c => c.Name == categoryName && c.Id != id);
    }

    public async Task<bool> DoesExist(string categoryName)
    {
        return await _dbContext.Categories.AnyAsync(c => c.Name == categoryName);
    }

    public async Task<bool> AreParentCategory(long id)
    {
        return await _dbContext.Categories.AnyAsync(c => c.ParentCategoryId == id);
    }

    public async Task<Category> Delete(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();

        return category;
    }
}