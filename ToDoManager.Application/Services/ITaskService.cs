using ToDoManager.Application.Dto;

namespace ToDoManager.Application.Services;

public interface ITaskService
{
    Task AddTask(int accountId, string name, string text, CancellationToken cancellationToken, DateTime? deadline = null, int? groupId = null);
    Task<TaskDto> RemoveTask(int taskId, CancellationToken cancellationToken);
    Task<IEnumerable<TaskDto>> GetTasksByGroup(int groupId, CancellationToken cancellationToken);
    Task<TaskDto> RenameTask(int id, string newName, CancellationToken cancellationToken);
    Task<TaskDto> EditTaskText(int id, string newText, CancellationToken cancellationToken);
    Task<TaskDto> AddDeadline(int taskId, DateTime deadline, CancellationToken cancellationToken);
    Task<TaskDto> CanceledTask(int taskId, CancellationToken cancellationToken);
}