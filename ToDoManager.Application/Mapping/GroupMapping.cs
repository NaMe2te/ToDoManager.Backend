using ToDoManager.Application.Dto;
using ToDoManager.DataAccess.Models;

namespace ToDoManager.Application.Mapping;

public static class GroupMapping
{
    public static GroupDto AdDto(this Group group)
        => new GroupDto(group.Id, group.Name);
}