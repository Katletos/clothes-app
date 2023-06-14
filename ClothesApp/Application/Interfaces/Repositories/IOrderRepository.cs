using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{ 
    Task<IList<Order>> FindByCondition(Expression<Func<Order, bool>> expression);
}