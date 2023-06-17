using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderTransactionsRepository
{
    Task<OrderTransaction> Insert(OrderTransaction orderTransaction);

    Task<IList<OrderTransaction>> GetByOrderId(long id);
}