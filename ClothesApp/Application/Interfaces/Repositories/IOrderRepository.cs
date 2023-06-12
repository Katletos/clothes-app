using System.IO.Enumeration;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order> GetLastUserOrder(Expression<Func<Order, bool>> expression);
    
    Task<IList<Order>> FindByCondition(Expression<Func<Order, bool>> expression);
}