using Application.Dtos.Users;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> DidUserBuyProduct(long userId, long productId);

    Task<bool> AreSameEmail(long userId, string email);

    Task<User> Delete(User user);

    Task<bool> DoesExist(string email);

    Task<bool> Login(UserLoginDto userLoginDto);

    Task<User> GetByEmail(string email);
}