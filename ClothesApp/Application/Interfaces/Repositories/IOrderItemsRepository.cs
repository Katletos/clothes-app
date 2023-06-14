using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderItemsRepository : IBaseRepository<OrderItem>
{
    Task<IList<OrderItem>> GetByOrderId(long id);
    
    Task<bool> DoesExistByProductId(long orderId, long id);

    Task<IList<OrderItem>> InsertRange(IList<OrderItem> orderItems);
}