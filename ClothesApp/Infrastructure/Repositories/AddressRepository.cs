using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ClothesAppContext _dbContext;

    public AddressRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Address> Insert(Address address)
    {
        _dbContext.Addresses.Add(address);
        await _dbContext.SaveChangesAsync();

        return address;
    }

    public async Task<Address> Update(Address address)
    {
        _dbContext.Addresses.Update(address);
        await _dbContext.SaveChangesAsync();

        return address;
    }

    public async Task<IList<Address>> GetAll()
    {
        return await _dbContext.Addresses.ToListAsync();
    }

    public async Task<Address> GetById(long id)
    {
        return await _dbContext.Addresses.FindAsync(id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Addresses.AnyAsync(b => b.Id == id);
    }

    public async Task<IList<Address>> FindByCondition(Expression<Func<Address, bool>> expression)
    {
        return await _dbContext.Addresses.Where(expression).ToListAsync();
    }

    public async Task<Address> Delete(Address address)
    {
        _dbContext.Addresses.Remove(address);
        await _dbContext.SaveChangesAsync();

        return address;
    }
}