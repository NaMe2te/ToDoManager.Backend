namespace ToDoManager.DataAccess.Models;

public class Group
{
    private readonly List<Task> _tasks;
        
    public Group(int id, string name, int accountId)
    {
        Id = id;
        Name = name;
        AccountId = accountId;
        _tasks = new List<Task>();
    }
    
    public int Id { get; init; }
    public string Name { get; private set; }
    public int AccountId { get; init; }

    public IReadOnlyCollection<Task> Tasks => _tasks;

    public void AddTasks(IEnumerable<Task> tasks)
    {
        _tasks.AddRange(tasks);
    }

    public void Rename(string newName)
    {
        Name = newName;
    }
}