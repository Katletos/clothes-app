using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderItemsRepository
{
    Task<IList<OrderItem>> GetByOrderId(long id);

    Task<IList<OrderItem>> InsertRange(IList<OrderItem> orderItems);
}