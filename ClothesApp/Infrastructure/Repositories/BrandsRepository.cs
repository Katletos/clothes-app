using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BrandsRepository : IBrandsRepository
{
    private readonly ClothesAppContext _dbContext;

    public BrandsRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }

    public async Task<Brand> InsertAsync(Brand brand)
    {
        await _dbContext.Brands.AddAsync(brand);
        await _dbContext.SaveChangesAsync();

        return brand;
    }

    public async Task<Brand> UpdateAsync(Brand brand)
    {
        _dbContext.Update(brand);
        await _dbContext.SaveChangesAsync();

        return brand;
    }

    public async Task<Brand> DeleteByIdAsync(long id)
    {
        var brand = await _dbContext.Brands.FindAsync(id);

        if (brand is not null)
        {
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
        }

        return brand;
    }

    public async Task<bool> IsExistAsync(long id)
    {
        return await _dbContext.Brands.AnyAsync(b => b.Id == id);
    }

    public async Task<bool> IsExistAsync(string brandName)
    {
        return await _dbContext.Brands.AnyAsync(b => b.Name == brandName);
    }

    public async Task<Brand> GetByIdAsync(long id)
    {
        return await _dbContext.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    public IQueryable<Brand> GetAll()
    {
        return _dbContext.Brands.AsQueryable().AsNoTracking();
    }
}