using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;
    
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<IList<UserDto>> GetAll()
    {
        var users = await _userRepository.GetAll();

        var userDto = _mapper.Map<IList<UserDto>>(users);

        return userDto;
    }

    public async Task<UserDto> GetById(long id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto> UpdateUserType(long id, UserType userType)
    {
        var exist = await _userRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }
        
        var user = await _userRepository.GetById(id);
        var userDto = _mapper.Map<User, UserDto>(user, opt =>
            opt.BeforeMap((src, _) => src.UserType = userType));

        await _userRepository.Update(user);

        return userDto;
    }

    public async Task<UserDto> UpdateAddress(long id, AddressInputDto addressInputDto)
    {
        var exist = await _userRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var user = await _userRepository.GetById(id);
        var userDto = _mapper.Map<User, UserDto>(user, opt =>
            opt.BeforeMap((src, _) => src.Id = id));

        await _userRepository.Update(user);

        return userDto;
    }
    
    public async Task<UserDto> DeleteById(long id)
    {
        var exist = await _userRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        var user = await _userRepository.GetById(id);
        await _userRepository.Delete(user);
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto> Add(UserInputDto userInputDto)
    {
        var exist = await _userRepository.DoesExist(userInputDto.Email);

        if (exist)
        {
            throw new BusinessRuleException(Messages.EmailUniqueConstraint);
        }

        var user = _mapper.Map<User>(userInputDto);
        await _userRepository.Insert(user);
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto> Update(long id, UserInputDto userInputDto)
    {
        var user = _mapper.Map<UserInputDto, User>(userInputDto, opt => 
        opt.AfterMap((_, dest) => dest.Id = id));

        var exist = await _userRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.NotFound);
        }

        await _userRepository.Update(user);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}