using Application.Dtos.Addresses;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly IMapper _mapper;

    public AddressService(IAddressRepository addressRepository, IUserRepository userRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<IList<AddressDto>> GetAddresses(long userId)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }
        
        var addresses = await _addressRepository.FindByCondition(a => a.UserId == userId);
        var addressesDto = _mapper.Map<IList<AddressDto>>(addresses);
        
        return addressesDto;
    }

    public async Task<bool> DoesAddressBelongToUser(long addressId, long userId)
    {
        var address = await _addressRepository.FindByCondition(a => a.Id == addressId && a.UserId == userId);
        
        return address.Count > 0;
    }
    
    public async Task<AddressDto> UpdateAddress(long userId, AddressInputDto addressInputDto)
    {
        var exist = await _userRepository.DoesExist(userId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        exist = await _addressRepository.DoesExist(addressInputDto.Id);

        if (!exist)
        {
            throw new NotFoundException(Messages.AddressNotFound);
        }

        var address = await _addressRepository.GetById(addressInputDto.Id);
        var addressDto = _mapper.Map<Address, AddressDto>(address, opt =>
            opt.BeforeMap((src, _) => src.AddressLine = addressInputDto.AddressLine));
        await _addressRepository.Update(address);
        
        return addressDto;
    }
    
    public async Task<AddressDto> DeleteAddress(long addressId)
    {
        var exist = await _addressRepository.DoesExist(addressId);

        if (!exist)
        {
            throw new NotFoundException(Messages.AddressNotFound);
        }

        var address = await _addressRepository.GetById(addressId);
        await _addressRepository.Delete(address);
        var addressDto = _mapper.Map<AddressDto>(address);

        return addressDto;
    }
}