using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICartItemsRepository
{
    Task<IList<CartItem>> GetByProductId(long productId);

    Task<IList<CartItem>> GetByUserId(long productId);

    Task<bool> DoesExist(long userId, long productId);

    Task<CartItem> Insert(CartItem item);

    Task<CartItem> Delete(CartItem item);

    Task<CartItem> Update(CartItem item);

    Task<CartItem> GetItem(long productId, long userId);

    Task UpdateQuantity(long productId, long incDecQuantity);

    Task<long> GetReservedProductQuantity(long productId);

    Task<long> GetAvailableProductQuantity(long productId);
}