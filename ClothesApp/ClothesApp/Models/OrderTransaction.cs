namespace ClothesApp.Models;

public class OrdersTransaction
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
