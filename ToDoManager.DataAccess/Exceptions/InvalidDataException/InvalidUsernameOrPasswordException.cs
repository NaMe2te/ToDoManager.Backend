namespace ToDoManager.DataAccess.Exceptions.InvalidDataException;

public class InvalidUsernameOrPasswordException : Exception
{
    public InvalidUsernameOrPasswordException()
        : base("Username or password is invalid") { }
}