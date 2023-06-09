namespace Application.Interfaces.Services;

public interface IAddressService
{
    Task<bool> DoesAddressBelongToUser(long addressId, long userId);
}