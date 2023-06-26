using Application.Dtos.Users;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly IJwtProvider _jwtProvider;

    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
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
            throw new NotFoundException(Messages.UserNotFound);
        }

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto> Add(RegisterUserDto registerUserDto)
    {
        var exist = await _userRepository.DoesExist(registerUserDto.Email);

        if (exist)
        {
            throw new BusinessRuleException(Messages.EmailUniqueConstraint);
        }

        var user = _mapper.Map<User>(registerUserDto);
        await _userRepository.Insert(user);
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<UserDto> Update(long id, UserInputInfoDto userInputInfoDto)
    {
        var exist = await _userRepository.DoesExist(id);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        var sameEmail = await _userRepository.AreSameEmail(id, userInputInfoDto.Email);

        if (sameEmail)
        {
            throw new BusinessRuleException(Messages.EmailUniqueConstraint);
        }

        var user = await _userRepository.GetById(id);
        var userDto = _mapper.Map<User, UserDto>(user, opt =>
            opt.BeforeMap((src, _) =>
            {
                src.FirstName = userInputInfoDto.FirstName;
                src.LastName = userInputInfoDto.LastName;
                src.Email = userInputInfoDto.Email;
                src.FirstName = userInputInfoDto.FirstName;
            }));
        await _userRepository.Update(user);

        return userDto;
    }

    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var exist = await _userRepository.DoesExist(userLoginDto.Email);

        if (!exist)
        {
            throw new NotFoundException(Messages.UserNotFound);
        }

        var login = await _userRepository.Login(userLoginDto);

        if (!login)
        {
            throw new BusinessRuleException(Messages.InvalidPassword);
        }

        var user = await _userRepository.GetByEmail(userLoginDto.Email);

        return _jwtProvider.Generate(user);
    }
}