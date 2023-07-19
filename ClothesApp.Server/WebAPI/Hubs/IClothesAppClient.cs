namespace WebAPI.Hubs;

public interface IClothesAppClient
{
    public Task UpdateProductViews(long count);

    public Task UpdateReservedQuantity(long productId);

    public Task UpdateAvailableQuantity(long productId);
}