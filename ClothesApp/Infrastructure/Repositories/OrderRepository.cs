using System.Linq.Expressions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ClothesAppContext _dbContext;

    public OrderRepository(ClothesAppContext context)
    {
        _dbContext = context;
    }

    public async Task<Order> Insert(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<Order> Update(Order order)
    {
        _dbContext.Update(order);
        await _dbContext.SaveChangesAsync();

        return order;
    }

    public async Task<IList<Order>> GetAll()
    {
        return await _dbContext.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<Order> GetById(long id)
    {
        return await _dbContext.Orders.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> DoesExist(long id)
    {
        return await _dbContext.Orders.AnyAsync(o => o.Id == id);
    }
    
    public async Task<IList<Order>> FindByCondition(Expression<Func<Order, bool>> expression)
    {
        return await _dbContext.Orders.Where(expression).ToListAsync();
    }
}