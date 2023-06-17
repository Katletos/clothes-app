namespace Application.Dtos.Categories;

public class CategoryInputDto
{
    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }
}