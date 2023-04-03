using ToDoManager.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.DataAccess.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account> Login(string username, string password, CancellationToken cancellationToken);
    Task<bool> IsUsernameContains(string username);
}