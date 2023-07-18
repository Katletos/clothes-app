namespace Application.Dtos.OrderItems;

public struct OrderItemDto
{
    public long ProductId { get; set; }

    public long OrderId { get; set; }

    public long Quantity { get; set; }

    public decimal Price { get; set; }
}