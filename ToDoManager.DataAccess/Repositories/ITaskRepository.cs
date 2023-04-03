using Task = ToDoManager.DataAccess.Models.Task;

namespace ToDoManager.DataAccess.Repositories;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksWithoutGroupByAccount(int accountId, CancellationToken cancellationToken);
    Task<IEnumerable<Task>> GetTasksByGroup(int groupId, CancellationToken cancellationToken);
}