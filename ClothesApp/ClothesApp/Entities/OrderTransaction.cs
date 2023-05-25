using ClothesApp.Enums;

namespace ClothesApp.Entities;

public class OrdersTransaction
{
    public long Id { get; set; }

    public long OrderId { get; set; }
    
    public OrderStatusType OrderStatus { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; }
}
