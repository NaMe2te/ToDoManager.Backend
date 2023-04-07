using System.Runtime.InteropServices;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Mapping;
using ToDoManager.DataAccess.Repositories;

namespace ToDoManager.Application.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task AddTask(int accountId, string name, string text, CancellationToken cancellationToken, DateTime? deadline = null, int? groupId = null)
    {
        
        var task = new DataAccess.Models.Task(default, name, text, accountId, deadline, groupId);
        await _taskRepository.CreateAsync(task, cancellationToken);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksWithoutGroupByAccount(int id, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetTasksWithoutGroupByAccount(id, cancellationToken);
        return tasks.Select(task => task.AdDto());
    }

    public async Task<TaskDto> RemoveTask(int taskId, CancellationToken cancellationToken)
    {
        var removedTask = await _taskRepository.GetModelAsync(taskId, cancellationToken);
        await _taskRepository.DeleteAsync(removedTask.Id, cancellationToken);
        return removedTask.AdDto();
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByGroup(int groupId, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetTasksByGroup(groupId, cancellationToken);
        return tasks.Select(task => task.AdDto());
    }

    public async Task<TaskDto> RenameTask(int id, string newName, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetModelAsync(id, cancellationToken);
        task.Rename(newName);
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return task.AdDto();
    }

    public async Task<TaskDto> EditTaskText(int id, string newText, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetModelAsync(id, cancellationToken);
        task.EditText(newText);
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return task.AdDto();
    }

    public async Task<TaskDto> AddDeadline(int taskId, DateTime deadline, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetModelAsync(taskId, cancellationToken);
        task.AddDeadline(deadline);
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return task.AdDto();
    }

    public async Task<TaskDto> CanceledTask(int taskId, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetModelAsync(taskId, cancellationToken);
        task.Complete();
        await _taskRepository.UpdateAsync(task, cancellationToken);
        return task.AdDto();
    }
}