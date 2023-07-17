using Application.Dtos.CartItem;

namespace Application.Interfaces.Services;

public interface ICartItemsService
{
    Task<IList<CartItemDto>> GetUserCartItems(long userId);

    Task<CartItemDto> Add(CartItemDto cartItemDto);

    Task<CartItemDto> Remove(CartItemDto cartItemDto);

    Task<CartItemDto> Drop(CartItemDto cartItemDto);

    Task<long> GetReservedProductQuantity(long productId);

    Task<long> GetAvailableProductQuantity(long productId);
}