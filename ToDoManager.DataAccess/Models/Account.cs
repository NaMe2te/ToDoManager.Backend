namespace ToDoManager.DataAccess.Models;

public class Account
{
    public Account(int id, string username, string password)
    {
        Id = id;
        Username = username;
        Password = password;
    }

    public int Id { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}