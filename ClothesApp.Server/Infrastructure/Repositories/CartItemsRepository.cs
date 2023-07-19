using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CartItemsRepository : ICartItemsRepository
{
    private readonly ClothesAppContext _dbContext;

    public CartItemsRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<CartItem>> GetByProductId(long productId)
    {
        return await _dbContext.CartItems.Where(ci => ci.ProductId == productId).ToListAsync();
    }

    public async Task<IList<CartItem>> GetByUserId(long userId)
    {
        return await _dbContext.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();
    }

    public async Task<bool> DoesExist(long userId, long productId)
    {
        return await _dbContext.CartItems.AnyAsync(ci => ci.UserId == userId && ci.ProductId == productId);
    }

    public async Task<CartItem> Insert(CartItem item)
    {
        _dbContext.CartItems.Add(item);
        await _dbContext.SaveChangesAsync();

        return item;
    }

    public async Task<CartItem> Delete(CartItem item)
    {
        _dbContext.CartItems.Remove(item);
        await _dbContext.SaveChangesAsync();

        return item;
    }

    public async Task<CartItem> Update(CartItem item)
    {
        _dbContext.Update(item);
        await _dbContext.SaveChangesAsync();

        return item;
    }

    public async Task<CartItem> GetItem(long productId, long userId)
    {
        return await _dbContext.CartItems.FirstAsync(p => p.ProductId == productId && p.UserId == userId);
    }

    public Task<long> GetReservedProductQuantity(long productId)
    {
        return Task.FromResult(_dbContext.CartItems
            .Where(ci => ci.ProductId == productId)
            .Sum(ci => ci.Quantity));
    }

    public Task<long> GetAvailableProductQuantity(long productId)
    {
        return _dbContext.Products
            .Where(ci => ci.Id == productId)
            .Select(p => p.Quantity)
            .SingleOrDefaultAsync();
    }
}