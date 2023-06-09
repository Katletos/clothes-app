using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderTransactionsRepository
{
    Task<OrderTransaction> Add(OrderTransaction orderTransaction);
    
    Task<IList<OrderTransaction>> GetByOrderId(long id);
}