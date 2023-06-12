using System.Linq.Expressions;
using Application.Dtos.OrderItems;
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

    public async Task UpdateRange(IList<Product> products)
    {
        _dbContext.Products.UpdateRange(products);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DoesExistRange(IList<long> ids)
    {
        var products = await _dbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync();

        return products.Count() == ids.Count();
    }

    public async Task<IList<Product>> GetRange(IList<long> ids)
    {
        return await _dbContext.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }

    public Task<Product> Insert(Product entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> Update(Product product)
    {
        _dbContext.Update(product);
        await _dbContext.SaveChangesAsync();

        return product;
    }

    public async Task<IList<Product>> GetAll()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetById(long id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Products.AnyAsync(p => p.Id == id);
    }
}