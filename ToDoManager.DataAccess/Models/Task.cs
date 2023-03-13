using Org.BouncyCastle.Crypto.Engines;

namespace ToDoManager.DataAccess.Models;

public class Task
{
    public Task(int id, string name, string text, DateTime? deadline = null, int? groupId = null, bool isCompleted = false)
    {
        Id = id;
        Name = name;
        Text = text;
        IsCompleted = isCompleted;
        Deadline = deadline;
    }

    public int Id { get; init; }
    public string Name { get; private set; }
    public string Text { get; private set; }
    public bool IsCompleted { get; private set; }
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

    public void Complete() => IsCompleted = true;
}