namespace Domain.Entities;

public class Product
{
    public long Id { get; set; }

    public long? BrandId { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public long Quantity { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Category Category { get; set; }

    public virtual ICollection<Media> Media { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; }

    public virtual ICollection<OrderItem> OrdersItems { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}