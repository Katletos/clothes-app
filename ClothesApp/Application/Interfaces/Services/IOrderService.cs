using Application.Dtos.OrderItems;
using Application.Dtos.Orders;
using Application.Dtos.OrderTransactions;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderDto> SubmitOrder(long id);
    
    Task<IList<OrderDto>> GetAll();

    Task<OrderDto> GetById(long id);

    Task<OrderDto> Add(OrderInputDto orderInputDto);
    
    Task<OrderItemDto> AddItem(long id, OrderItemInputDto orderItemInputDto);

    Task<OrderDto> UpdateStatus(long id);

    Task<OrderDto> DeleteById(long id);

    Task<IList<OrderTransactionsDto>> GetOrderHistoryByOrderId(long id);

    Task<IList<OrderItemDto>> GetOrderItemsByOrderId(long orderId);

    Task<decimal> CalcOrderPrice(long id);

    Task<decimal> CalcProductPrice(OrderItemInputDto orderItemInputDto);

    Task ReserveProduct(OrderItemInputDto orderItemInputDto);
}