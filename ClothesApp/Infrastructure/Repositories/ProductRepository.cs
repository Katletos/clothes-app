using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ClothesAppContext _dbContext;

    public ProductRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }

    public Task<Product> InsertAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Product> UpdateAsync(Product entity)
    {
        throw new NotImplementedException();
    }

    public Task<Product> DeleteByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsExistAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsExistAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Product> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AnyProductOfBrandIdExists(long brandId)
    {
        return await _dbContext.Products.AnyAsync(p => p.BrandId == brandId);
    }
}