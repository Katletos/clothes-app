using Application.Dtos.Products;

namespace Application.Dtos.CartItem;

public struct CartItemDto
{
    public long ProductId { get; set; }

    public long UserId { get; set; }

    public long Quantity { get; set; }

    public ProductDto Product { get; set; }
}