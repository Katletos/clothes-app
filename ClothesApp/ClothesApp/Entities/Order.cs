using ClothesApp.Enums;

namespace ClothesApp.Entities;

public class Order
{
    public long Id { get; set; }
    
    public OrderStatusType OrderStatus { get; set; }

    public long UserId { get; set; }

    public decimal Price { get; set; }

    public long AddressId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Address Address { get; set; }

    public virtual ICollection<OrderItem> OrdersItems { get; set; }

    public virtual ICollection<OrdersTransaction> OrdersTransactions { get; set; }

    public virtual User User { get; set; }
}
