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

    public Task<IList<Product>> FindByCondition(Expression<Func<Product, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<Product> Insert(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Product> Update(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Product>> GetAll()
    {
        throw new NotImplementedException();
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