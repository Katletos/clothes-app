using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderItemsRepository : IOrderItemsRepository
{
    private readonly ClothesAppContext _dbContext;

    public OrderItemsRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderItem> Insert(OrderItem orderItem)
    {
        _dbContext.OrdersItems.Add(orderItem);
        await _dbContext.SaveChangesAsync();

        return orderItem;
    }

    public async Task<OrderItem> Update(OrderItem orderItem)
    {
        _dbContext.OrdersItems.Update(orderItem);
        await _dbContext.SaveChangesAsync();

        return orderItem;
    }

    public Task<IList<OrderItem>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<OrderItem> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesExist(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<OrderItem>> GetByOrderId(long id)
    {
        return await _dbContext.OrdersItems.Where(o => o.OrderId == id).ToListAsync();
    }

    public async Task<bool> DoesExistByGetByProductId(long orderId,long id)
    {
        return await _dbContext.OrdersItems.AnyAsync(oi => oi.OrderId ==orderId && oi.ProductId == id);
    }

    public async Task<IList<OrderItem>> InsertRange(IList<OrderItem> orderItems)
    {
        _dbContext.OrdersItems.AddRange(orderItems);
        await _dbContext.SaveChangesAsync();

        return orderItems;
    }
}