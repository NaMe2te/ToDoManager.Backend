namespace ToDoManager.Application.Dto;

public record TaskDto(int Id, string Name, string Text, bool IsCompeted, DateTime? Deadline, int? GroupId);