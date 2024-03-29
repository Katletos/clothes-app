namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T> Insert(T entity);

    Task<T> Update(T entity);

    Task<IList<T>> GetAll();

    Task<T> GetById(long id);

    Task<bool> DoesExist(long id);
}