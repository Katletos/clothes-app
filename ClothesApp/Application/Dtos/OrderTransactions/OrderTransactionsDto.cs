using Domain.Enums;

namespace Application.Dtos.OrderTransactions;

public class OrderTransactionsDto
{
    public long Id { get; set; }

    public long OrderId { get; set; }
    
    public OrderStatusType OrderStatus { get; set; }

    public DateTime UpdatedAt { get; set; }
}