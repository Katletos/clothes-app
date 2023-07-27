using Domain.Enums;

namespace Domain.Entities;

public class OrderTransaction
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public OrderStatusType OrderStatus { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; }
}