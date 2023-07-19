namespace Application.Dtos.Categories;

public class CategoryTree
{
    public long Id { get; set; }

    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }

    public IList<CategoryTree> Children { get; set; }
}