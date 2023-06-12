using Application.Dtos.Category;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<bool> AreSameName(long id, string categoryName);
    
    Task<bool> DoesExist(string categoryName);

    Task<bool> AreParentCategory(long id);

    Task<Category> Delete(Category category);
}