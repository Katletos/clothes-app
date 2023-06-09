namespace Application.Interfaces.Services;

public interface IAddressService
{
    Task<bool> DoesAddressBelongUser(long addressId, long userId);
}