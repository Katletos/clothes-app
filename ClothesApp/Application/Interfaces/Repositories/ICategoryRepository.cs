using System.Linq.Expressions;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IList<Category>> FindByCondition(Expression<Func<Category, bool>> expression);

    Task<bool> AreSameName(long id, string categoryName);

    Task<bool> DoesExist(string categoryName);

    Task<bool> AreParentCategory(long id);

    Task<Category> Delete(Category category);
}