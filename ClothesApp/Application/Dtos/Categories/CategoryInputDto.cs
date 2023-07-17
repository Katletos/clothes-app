namespace Application.Dtos.Categories;

public struct CategoryInputDto
{
    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }
}