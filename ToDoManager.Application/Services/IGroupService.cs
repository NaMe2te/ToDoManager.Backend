using ToDoManager.Application.Dto;

namespace ToDoManager.Application.Services;

public interface IGroupService
{
    Task AddGroup(string groupName, CancellationToken cancellationToken);
    Task RemoveGroup(int id, CancellationToken cancellationToken);
    Task<GroupDto> GetGroup(int id, CancellationToken cancellationToken);
    Task<IEnumerable<GroupDto>> GetAllGroups(CancellationToken cancellationToken);
    Task<GroupDto> RenameGroup(int id, string newGroupName, CancellationToken cancellationToken);
}