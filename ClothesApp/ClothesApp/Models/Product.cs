namespace ClothesApp.Models;

public class Product
{
    public long Id { get; set; }

    public long BrandId { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public long Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Media> Media { get; set; } = new List<Media>();

    public virtual ICollection<OrderItem> OrdersItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
