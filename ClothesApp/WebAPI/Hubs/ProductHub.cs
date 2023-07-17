using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs;

public class ProductHub : Hub<IClothesAppClient>
{
    private readonly ProductViews _productViews;

    private readonly ICartItemsService _cartItemsService;

    public ProductHub(ProductViews productViews, ICartItemsService cartItemsService)
    {
        _productViews = productViews;
        _cartItemsService = cartItemsService;
    }

    public async Task EnterProduct(long productId, long userId)
    {
        _productViews.IncrementView(productId, userId);

        var groupName = "product" + productId;
        var count = _productViews.GetCount(productId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).UpdateProductViews(count);
    }

    public async Task LeaveProduct(long productId, long userId)
    {
        _productViews.DecrementView(productId, userId);

        var groupName = "product" + productId;
        var count = _productViews.GetCount(productId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).UpdateProductViews(count);
    }

    public async Task GetReservedQuantity(long productId)
    {
        var groupName = "product" + productId;
        var reservedQuantity = await _cartItemsService.GetReservedProductQuantity(productId);
        await Clients.Group(groupName).UpdateReservedQuantity(reservedQuantity);
    }

    public async Task GetAvailableQuantity(long productId)
    {
        var groupName = "product" + productId;
        var availableQuantity = await _cartItemsService.GetAvailableProductQuantity(productId);
        await Clients.Group(groupName).UpdateAvailableQuantity(availableQuantity);
    }

    public async Task EnterCart(long[] productIds, long userId)
    {
        foreach (var productId in productIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "product" + productId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "cart" + userId);
    }

    public async Task LeaveCart(long[] productIds, long userId)
    {
        foreach (var productId in productIds)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "product" + productId);
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "cart" + userId);
    }

}