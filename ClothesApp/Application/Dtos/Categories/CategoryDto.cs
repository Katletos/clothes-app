namespace Application.Dtos.Categories;

public class CategoryDto
{
    public long Id { get; set; }

    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }
}