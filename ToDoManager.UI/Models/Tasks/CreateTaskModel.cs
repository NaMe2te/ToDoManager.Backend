namespace ToDoManager.UI.Models.Tasks;

public record CreateTaskModel(string Name, string Text, DateTime? Deadline, int? GroupId);