using ToDoManager.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.DataAccess.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> FindAccountByUsername(string username, CancellationToken cancellationToken);
}