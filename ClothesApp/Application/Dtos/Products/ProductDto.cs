namespace Application.Dtos.Products;

public struct ProductDto
{
    public long Id { get; set; }

    public long? BrandId { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public long Quantity { get; set; }

    public DateTime CreatedAt { get; set; }
}