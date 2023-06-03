using Domain.Enums;

namespace Application.Dtos.Orders;

public class OrderInputDto
{
    public OrderStatusType OrderStatus { get; set; }

    public long UserId { get; set; }

    public decimal Price { get; set; }

    public long AddressId { get; set; }

    public DateTime CreatedAt { get; set; }
}