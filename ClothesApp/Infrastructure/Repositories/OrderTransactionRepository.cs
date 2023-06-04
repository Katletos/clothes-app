using Application.Interfaces.Repositories;
using Domain.Entities;

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
}