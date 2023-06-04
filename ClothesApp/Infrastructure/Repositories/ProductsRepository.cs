using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ClothesAppContext _dbContext;

    public ProductsRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AnyProductOfBrandIdExists(long brandId)
    {
        return await _dbContext.Products.AnyAsync(p => p.BrandId == brandId);
    }

    public async Task<bool> DoesExist(string name)
    {
        return await _dbContext.Products.AnyAsync(p => p.Name == name);
    }

    public async Task<IList<Product>> FindByCondition(Expression<Func<Product, bool>> expression)
    {
        return await _dbContext.Set<Product>().Where(expression).ToListAsync();
    }

    public Task<Product> Insert(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Product> Update(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Product>> GetAll()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public Task<Product> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Products.AnyAsync(p => p.Id == id);
    }
}