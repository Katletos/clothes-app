using Application.Dtos.Addresses;
using Application.Dtos.Users;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAll();

    Task<UserDto> GetById(long userId);

    Task<AddressDto> UpdateAddress(long userId, AddressInputDto addressInputDto);

    Task<AddressDto> DeleteAddress(long addressId);

    Task<IList<AddressDto>> GetAddresses(long userId);
    
    Task<UserDto> DeleteById(long userId);

    Task<UserDto> Add(RegisterUserDto registerUserDto);

    Task<UserDto> Update(long userId, UserInputInfoDto userInputInfoDto);

    Task<bool> Login(UserLoginDto userLoginDto);
}