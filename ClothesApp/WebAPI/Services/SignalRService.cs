using Application.Dtos.CartItem;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Services;

public class SignalRService : ISignalRService
{
    private readonly IHubContext<ProductHub> _productHub;

    private readonly ICartItemsService _cartItemsService;

    public SignalRService(IHubContext<ProductHub> productHub, ICartItemsService cartItemsService)
    {
        _productHub = productHub;
        _cartItemsService = cartItemsService;
    }

    public async Task UpdateReservedQuantity(long productId)
    {
        var reservedQuantity = await _cartItemsService.GetReservedProductQuantity(productId);

        await _productHub.Clients
            .Group("product" + productId)
            .SendAsync("UpdateReservedQuantity", reservedQuantity);
    }

    public async Task UpdateAvailableQuantity(long productId)
    {
        var availableQuantity = await _cartItemsService.GetAvailableProductQuantity(productId);

        await _productHub.Clients
            .Group("product" + productId)
            .SendAsync("UpdateAvailableQuantity", availableQuantity);
    }

    public async Task UpdateCart(long userId)
    {
        await _productHub.Clients
            .Group("cart" + userId)
            .SendAsync("UpdateCart");
    }

    public async Task DeleteExpiredCartItem(CartItemDto cartItemDto)
    {
        var cartItem = await _cartItemsService.Drop(cartItemDto);

        await UpdateReservedQuantity(cartItem.ProductId);
        await UpdateCart(cartItem.UserId);
    }
}