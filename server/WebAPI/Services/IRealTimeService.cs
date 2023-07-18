namespace WebAPI.Services;

public interface IRealTimeService
{
    Task DeleteExpiredCartItem(long productId, long userId);

    Task UpdateReservedQuantity(long productId);

    Task UpdateAvailableQuantity(long productId);

    Task UpdateCart(long userId);
}