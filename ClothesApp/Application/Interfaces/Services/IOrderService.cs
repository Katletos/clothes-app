using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderDto> GetById(long id);

    Task<OrderDto> Add(OrderInputDto orderInputDto);
    
    Task<OrderDto> UpdateStatus(long id, OrderStatusType newOrderStatus);
    
    Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id);

    Task<IList<OrderItemDto>> GetOrderItemsByOrderId(long orderId);

    Task<IList<OrderDto>> GetOrdersByStatus(OrderStatusType status);
}