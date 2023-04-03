using ToDoManager.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.DataAccess.Repositories;

public interface IGroupRepository : IRepository<Group>
{
    Task<IEnumerable<Group>> GetAllByAccount(int accountId, CancellationToken cancellationToken);
}