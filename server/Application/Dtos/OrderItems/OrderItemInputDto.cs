namespace Application.Dtos.OrderItems;

public struct OrderItemInputDto
{
    public long ProductId { get; set; }

    public long Quantity { get; set; }
}