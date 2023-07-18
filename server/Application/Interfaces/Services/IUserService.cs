using Application.Dtos.Users;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<IList<UserDto>> GetAll();

    Task<UserDto> GetById(long userId);

    Task<UserDto> Add(RegisterUserDto registerUserDto);

    Task<UserDto> Update(long userId, UserInputInfoDto userInputInfoDto);

    Task<DtoToLocalStorage> Login(UserLoginDto userLoginDto);
}