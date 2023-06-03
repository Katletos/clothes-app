using Application.Dtos.Addresses;
using Application.Dtos.Users;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAll();

    Task<UserDto> GetById();

    Task<UserDto> UpdateUserType(long id, UserType userType);

    Task<UserDto> UpdateAddress(long id, AddressInputDto addressInputDto);

    Task<UserDto> DeleteById(long id);

    Task<UserDto> Add(UserInputDto userInputDto);

    Task<UserDto> Update(long id, UserInputDto userInputDto);
}