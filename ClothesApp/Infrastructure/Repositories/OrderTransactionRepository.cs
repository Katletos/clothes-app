using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderTransactionRepository : IOrderTransactionsRepository
{
    private readonly ClothesAppContext _dbContext;

    public OrderTransactionRepository(ClothesAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderTransaction> Add(OrderTransaction orderTransaction)
    {
        _dbContext.OrdersTransactions.Add(orderTransaction);
        await _dbContext.SaveChangesAsync();
        
        return orderTransaction;
    }
    
    public async Task<IList<OrderTransaction>> GetByOrderId(long id)
    {
        return await _dbContext.OrdersTransactions.Where(ot => ot.OrderId == id).ToListAsync();
    }
}