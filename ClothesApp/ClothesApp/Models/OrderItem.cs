namespace ClothesApp.Models;

public class OrderItem
{
    public long ProductId { get; set; }

    public long OrderId { get; set; }

    public long Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
