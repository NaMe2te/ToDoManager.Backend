namespace ToDoManager.UI.Models;

public record CreateTaskModel(int AccountId, string Name, string Text, DateTime? Deadline, int? GroupId);