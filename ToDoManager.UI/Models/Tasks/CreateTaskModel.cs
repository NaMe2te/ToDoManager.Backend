namespace ToDoManager.UI.Models.Tasks;

public record CreateTaskModel(int AccountId, string Name, string Text, DateTime? Deadline, int? GroupId);