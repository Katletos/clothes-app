namespace ClothesApp.Models;

public class Category
{
    public long Id { get; set; }

    public long? ParentCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
