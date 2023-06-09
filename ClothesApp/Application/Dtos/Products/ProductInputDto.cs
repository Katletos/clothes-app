namespace Application.Dtos.Products;

public class ProductInputDto
{
    public long BrandId { get; set; }

    public long CategoryId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public long Quantity { get; set; }
}