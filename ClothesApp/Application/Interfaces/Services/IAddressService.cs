using Application.Dtos.Addresses;

namespace Application.Interfaces.Services;

public interface IAddressService
{
    Task<bool> DoesAddressBelongToUser(long addressId, long userId);
    
    Task<AddressDto> UpdateAddress(long userId, AddressInputDto addressInputDto);

    Task<AddressDto> DeleteAddress(long addressId);

    Task<IList<AddressDto>> GetAddresses(long userId);
}