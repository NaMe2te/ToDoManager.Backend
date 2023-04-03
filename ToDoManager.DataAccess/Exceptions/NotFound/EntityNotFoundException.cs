namespace ToDoManager.DataAccess.Exceptions.NotFound;

public class EntityNotFoundException<T> : Exception
{
    public EntityNotFoundException(int id) : base(($"{typeof(T).Name} with id {id} was not found.")) { }
}