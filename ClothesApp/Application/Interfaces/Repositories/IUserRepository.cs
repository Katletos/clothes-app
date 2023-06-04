using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> Delete(User user);

    Task<bool> DoesExist(string email);
}