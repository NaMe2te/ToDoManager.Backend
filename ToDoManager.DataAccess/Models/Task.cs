namespace ToDoManager.DataAccess.Models;

public class Task
{
    public Task(int id, string name, string text, int accountId, DateTime? deadline = null, int? groupId = null, bool isCompleted = false)
    {
        Id = id;
        Name = name;
        Text = text;
        IsCompleted = isCompleted;
        AccountId = accountId;
        Deadline = deadline;
        GroupId = groupId;
    }

    public int Id { get; init; }
    public string Name { get; private set; }
    public string Text { get; private set; }
    public bool IsCompleted { get; private set; }
    public int AccountId { get; init; }
    public DateTime? Deadline { get; private set; }
    public int? GroupId { get; private set; }

    public void Rename(string newName)
    {
        Name = newName;
    }

    public void EditText(string newText)
    {
        Text = newText;
    }
    
    public DateTime AddDeadline(DateTime deadline)
    {
        Deadline = deadline;
        return deadline;
    }

    public void AddTaskToGroup(int groupId)
    {
        GroupId = groupId;
    }

    public void ChangeStatus(bool status) => IsCompleted = status;
}