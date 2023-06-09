using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<bool> DoesAddressBelongUser(long addressId, long userId)
    {
        var address = await _addressRepository.FindByCondition(a => a.Id == addressId && a.UserId ==userId);
        
        return address.Count > 0;
    }
}