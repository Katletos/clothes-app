using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<IList<Address>> FindByCondition(Expression<Func<Address, bool>> expression);

    Task<bool> DoesAddressBelongToUser(long addressId, long userId);

    Task<Address> Delete(Address address);
}