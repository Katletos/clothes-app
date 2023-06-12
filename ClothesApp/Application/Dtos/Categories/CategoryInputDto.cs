namespace Application.Dtos.Category;

public class CategoryInputDto
{
    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }
}