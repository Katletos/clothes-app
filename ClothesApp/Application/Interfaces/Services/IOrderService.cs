using Application.Dtos.Orders;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IOrderService
{
    Task<IList<OrderDto>> GetAll();

    Task<OrderDto> GetById(long id);

    Task<OrderDto> Add(OrderInputDto orderInputDto);

    Task<OrderDto> UpdateStatus(long id, OrderStatusType statusType);

    Task<OrderDto> DeleteById(long id);

    Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id);

    Task<IList<OrderItemDto>> GetOrderItemsByOrderId(long orderId);
}