using Application.Dtos.OrderItems;

namespace Application.Dtos.Orders;

public struct OrderInputDto
{
    public long UserId { get; set; }

    public long AddressId { get; set; }

    public IList<OrderItemInputDto> OrderItems { get; set; }
}