namespace ClothesApp.Entities;

public class Category
{
    public long Id { get; set; }

    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Category> ChildCategory { get; set; } = new List<Category>();

    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<SectionCategory> SectionCategories { get; } = new List<SectionCategory>();
}
