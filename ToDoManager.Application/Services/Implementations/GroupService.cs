using ToDoManager.Application.Dto;
using ToDoManager.Application.Mapping;
using ToDoManager.DataAccess.Models;
using ToDoManager.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.Application.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITaskRepository _taskRepository;

    public GroupService(IGroupRepository groupRepository, ITaskRepository taskRepository)
    {
        _groupRepository = groupRepository;
        _taskRepository = taskRepository;
    }

    public async Task AddGroup(int accountId, string groupName, CancellationToken cancellationToken)
    {
        Group group = new Group(default, groupName, accountId);
        await _groupRepository.CreateAsync(group, cancellationToken);
    }

    public async Task<GroupDto> GetGroup(int id, CancellationToken cancellationToken)
    {
        Group group = await _groupRepository.GetModelAsync(id, cancellationToken);
        var tasks = await _taskRepository.GetTasksByGroup(group.Id, cancellationToken);
        group.AddTasks(tasks);
        return group.AdDto();
    }

    public async Task<IEnumerable<GroupDto>> GetAllGroups(CancellationToken cancellationToken)
    {
        List<Group> groups = (await _groupRepository.GetAllAsync(cancellationToken)).ToList();
        foreach (var group in groups)
        {
            var tasks = await _taskRepository.GetTasksByGroup(group.Id, cancellationToken);
            group.AddTasks(tasks);
        }
        
        return groups.Select(group => group.AdDto());
    }

    public async Task RemoveGroup(int id, CancellationToken cancellationToken)
    {
        var tasksIdByGroup = (await _taskRepository.GetTasksByGroup(id, cancellationToken)).Select(task => task.Id).ToList();
        foreach (var taskId in tasksIdByGroup)
        {
            await _taskRepository.DeleteAsync(taskId, cancellationToken);
        }
        
        await _groupRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<GroupDto> RenameGroup(int id, string newGroupName, CancellationToken cancellationToken)
    {
        Group group = await _groupRepository.GetModelAsync(id, cancellationToken);
        group.Rename(newGroupName);
        await _groupRepository.UpdateAsync(group, cancellationToken);
        return group.AdDto();
    }
}