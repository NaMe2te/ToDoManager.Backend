namespace ToDoManager.DataAccess.Repositories;

public interface IRepository<T> 
    where T : class
{
    Task CreateAsync(T model, CancellationToken cancellationToken);
    Task UpdateAsync(T model, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<T> GetModelAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}