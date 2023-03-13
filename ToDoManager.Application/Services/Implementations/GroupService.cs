using ToDoManager.Application.Dto;
using ToDoManager.Application.Mapping;
using ToDoManager.DataAccess.Models;
using ToDoManager.DataAccess.Repositories;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.Application.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task AddGroup(string groupName, CancellationToken cancellationToken)
    {
        Group group = new Group(default, groupName);
        await _groupRepository.CreateAsync(group, cancellationToken);
    }

    public async Task<GroupDto> GetGroup(int id, CancellationToken cancellationToken)
    {
        Group group = await _groupRepository.GetModelAsync(id, cancellationToken);
        return group.AdDto();
    }

    public async Task RemoveGroup(int id, CancellationToken cancellationToken)
    {
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