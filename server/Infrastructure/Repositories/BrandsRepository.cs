using Application.Interfaces.Repositories;
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

    public async Task<Brand> Insert(Brand brand)
    {
        _dbContext.Brands.Add(brand);
        await _dbContext.SaveChangesAsync();

        return brand;
    }

    public async Task<Brand> Update(Brand brand)
    {
        _dbContext.Update(brand);
        await _dbContext.SaveChangesAsync();

        return brand;
    }

    public async Task<Brand> Delete(Brand brand)
    {
        _dbContext.Brands.Remove(brand);
        await _dbContext.SaveChangesAsync();

        return brand;
    }

    public async Task<Brand> GetById(long id)
    {
        return await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> DoesExist(string brandName)
    {
        return await _dbContext.Brands.AnyAsync(b => b.Name == brandName);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Brands.AnyAsync(b => b.Id == id);
    }

    public async Task<IList<Brand>> GetAll()
    {
        return await _dbContext.Brands.AsNoTracking().ToListAsync();
    }
}