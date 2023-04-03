using Task = ToDoManager.DataAccess.Models.Task;

namespace ToDoManager.DataAccess.Repositories;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksWithoutGroup(CancellationToken cancellationToken);
    Task<IEnumerable<Task>> GetTasksByGroup(int groupId, CancellationToken cancellationToken);
    Task<IEnumerable<Task>> GetCompletedTasks(CancellationToken cancellationToken);
}