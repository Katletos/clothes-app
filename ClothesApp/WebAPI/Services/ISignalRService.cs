using Application.Dtos.CartItem;

namespace WebAPI.Services;

public interface ISignalRService
{
    //Task DeleteExpiredCartItem(int id);
    Task UpdateReservedQuantity(long productId);

    Task UpdateAvailableQuantity(long productId);

    //Task UpdateCart(int userId);
    //Task UpdateCartItemQuantity(int userId, int cartItemId, int quantity);
    Task UpdateCart(long userId);

    Task DeleteExpiredCartItem(CartItemDto cartItemDto);
}