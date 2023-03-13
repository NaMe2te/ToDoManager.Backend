namespace ToDoManager.DataAccess.Models;

public class Group
{
    public Group(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; init; }
    public string Name { get; private set; }

    public void Rename(string newName)
    {
        Name = newName;
    }
}