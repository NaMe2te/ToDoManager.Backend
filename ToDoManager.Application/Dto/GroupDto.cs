namespace ToDoManager.Application.Dto;

public record GroupDto(int Id, string Name, IEnumerable<TaskDto> Tasks);