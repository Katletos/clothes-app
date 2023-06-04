namespace Domain.Entities;

public class Category
{
    public long Id { get; set; }

    public long ParentCategoryId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Category> ChildCategory { get; set; }

    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; }

    public virtual ICollection<Section> Sections { get; set; }

    public virtual ICollection<SectionCategory> SectionCategories { get; set; }
}
