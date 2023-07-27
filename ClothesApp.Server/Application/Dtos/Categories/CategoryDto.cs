namespace Application.Dtos.Categories;

public struct CategoryDto
{
    public long Id { get; set; }

    public long? ParentCategoryId { get; set; }

    public string Name { get; set; }
}