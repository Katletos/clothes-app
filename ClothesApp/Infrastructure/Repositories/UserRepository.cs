using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ClothesAppContext _dbContext;

    public UserRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }

    public async Task<User> Insert(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> Update(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<IList<User>> GetAll()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Users.AnyAsync(u => u.Id == id);
    }
}