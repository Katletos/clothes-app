using Application.Dtos.CartItem;

namespace Application.Interfaces.Services;

public interface ICartItemsService
{
    Task<IList<CartItemDto>> GetUserCartItems(long userId);

    Task<CartItemDto> Add(long productId, long userId);

    Task<CartItemDto> Update(long productId, long userId, long newQuantity);

    Task<CartItemDto> Delete(long productId, long userId);

    Task<long> GetReservedProductQuantity(long productId);

    Task<long> GetAvailableProductQuantity(long productId);
}