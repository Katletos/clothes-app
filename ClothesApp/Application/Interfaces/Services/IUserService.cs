using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAll();

    Task<UserDto> GetById(long id);

    Task<AddressDto> UpdateAddress(long userId, AddressInputDto addressInputDto);

    Task<UserDto> DeleteById(long id);

    Task<UserDto> Add(UserInputInfoDto userInputInfoDto);

    Task<UserDto> Update(long id, UserInputInfoDto userInputInfoDto);

    Task<bool> Login(UserLoginDto userLoginDto);
}