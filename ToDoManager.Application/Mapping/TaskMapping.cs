using ToDoManager.Application.Dto;
using Task = ToDoManager.DataAccess.Models.Task;

namespace ToDoManager.Application.Mapping;

public static class TaskMapping
{
    public static TaskDto AdDto(this Task task)
        => new TaskDto(task.Id, task.Name, task.Text, task.IsCompleted, task.Deadline, task.GroupId);
}