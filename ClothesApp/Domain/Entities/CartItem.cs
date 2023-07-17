namespace Domain.Entities;

public class CartItem
{
    public long ProductId { get; set; }

    public long UserId { get; set; }

    public long Quantity { get; set; }

    public virtual User User { get; set; }

    public virtual Product Product { get; set; }
}