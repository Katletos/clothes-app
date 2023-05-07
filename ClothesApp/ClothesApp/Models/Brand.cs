namespace ClothesApp.Models;

public class Brand
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
