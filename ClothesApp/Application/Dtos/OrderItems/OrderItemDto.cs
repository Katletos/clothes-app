namespace Application.Dtos.OrderItems;

public class OrderItemDto
{
    public long ProductId { get; set; }

    public long OrderId { get; set; }

    public long Quantity { get; set; }

    public decimal Price { get; set; }
}