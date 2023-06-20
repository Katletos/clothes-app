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

    public async Task<AddressDto> AddAddress(AddAddressDto addAddressDto)
    {
        var exist = await _userRepository.DoesExist(addAddressDto.UserId);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        var address = _mapper.Map<Address>(addAddressDto);
        await _addressRepository.Insert(address);
        var addressDto = _mapper.Map<AddressDto>(address);

        return addressDto;
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

        var belong = await _addressRepository.DoesAddressBelongToUser(addressInputDto.Id, userId);

        if (!belong)
        {
            throw new BusinessRuleException(Messages.AddressUserConstraint);
        }

        var address = await _addressRepository.GetById(addressInputDto.Id);
        address.AddressLine = addressInputDto.AddressLine;
        var addressDto = _mapper.Map<Address, AddressDto>(address);
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