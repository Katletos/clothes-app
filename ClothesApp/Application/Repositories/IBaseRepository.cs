namespace Application.Repositories;

public interface IBaseRepository<T> 
{
    Task<T> InsertAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task<T> DeleteByIdAsync(long id);
    
    Task<bool> IsExistAsync(long id);
    
    Task<bool> IsExistAsync(string name);
    
    Task<T> GetByIdAsync(long id);
    
    IQueryable<T> GetAll();
}