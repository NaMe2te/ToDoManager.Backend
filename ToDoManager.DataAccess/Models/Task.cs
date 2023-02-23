namespace ToDoManager.DataAccess.Models;

public class Task
{
    public Task(int id, string text, DateTime? deadline = null)
    {
        Id = id;
        Text = text;
        IsCompleted = false;
        Deadline = deadline;
    }

    public int Id { get; init; }
    public string Text { get; init; }
    public bool IsCompleted { get; private set; }
    public DateTime? Deadline { get; private set; }

    public DateTime AddDeadline(DateTime deadline)
    {
        Deadline = deadline;
        return deadline;
    }
}