namespace ToDoManager.UI.Models;

public record CreateTaskModel(string Name, string Text, string? Deadline, int? GroupId);