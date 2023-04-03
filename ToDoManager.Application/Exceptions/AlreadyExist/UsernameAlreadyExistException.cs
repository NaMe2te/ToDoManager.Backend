namespace ToDoManager.Application.Exceptions.AlreadyExist;

public class UsernameAlreadyExistException : Exception
{
    public UsernameAlreadyExistException(string username)
        : base($"Username {username} already exist") { }
}