namespace Application.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T> InsertAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task<IReadOnlyCollection<T>> GetAllAsync();
}