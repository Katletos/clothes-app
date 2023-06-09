using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAddressRepository : IBaseRepository<Address>
{
    Task<IList<Address>> FindByCondition(Expression<Func<Address, bool>> expression);
}