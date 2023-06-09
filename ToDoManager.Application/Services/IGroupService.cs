﻿using ToDoManager.Application.Dto;

namespace ToDoManager.Application.Services;

public interface IGroupService
{
    Task AddGroup(int accountId, string groupName, CancellationToken cancellationToken);
    Task<IEnumerable<GroupDto>> GetAllByAccount(int id, CancellationToken cancellationToken);
    Task RemoveGroup(int id, CancellationToken cancellationToken);
    Task<GroupDto> RenameGroup(int id, string newGroupName, CancellationToken cancellationToken);
}