using System.Net;
using MySql.Data.MySqlClient;
using Group = ToDoManager.DataAccess.Models.Group;

namespace ToDoManager.DataAccess.Repositories.Implementations;

public class GroupRepository : IGroupRepository
{
    private readonly string _connectionString;

    private readonly ITaskRepository _taskRepository;
    
    public GroupRepository(string connectionString, ITaskRepository taskRepository)
    {
        _connectionString = connectionString;
        _taskRepository = taskRepository;
    }
    
    public async Task CreateAsync(Group model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var createQuery = $"insert into {connection.Database}.groups(name)"
                              + $" value ('{model.Name}');";
            await using (var command = new MySqlCommand(createQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task UpdateAsync(Group model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var updateQuery = $"update {connection.Database}.groups"
                              + $" set name = '{model.Name}'"
                              + $" where id = {model.Id};";
            await using (var command = new MySqlCommand(updateQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var deleteQuery = $"delete from {connection.Database}.groups where id = {id};";
            await using (var command = new MySqlCommand(deleteQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
            
        }
    }

    public async Task<Group> GetModelAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            Group? group = null;
            await connection.OpenAsync(cancellationToken);
            string getQuery = $"select * from {connection.Database}.groups where id = {id};";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        group = new Group(groupId, name);
                    }
                }
            }

            if (group is null)
                throw new Exception("группы с таким id не было найдено");

            var tasks = await _taskRepository.GetTasksByGroup(group.Id, cancellationToken);
            group.AddTasks(tasks);

            return group;
        }
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            var groups = new List<Group>();
            await connection.OpenAsync(cancellationToken);
            string getQuery = $"select * from {connection.Database}.groups;";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        var group = new Group(groupId, name);
                        group.AddTasks(await _taskRepository.GetTasksByGroup(group.Id, cancellationToken));
                        groups.Add(group);
                        
                    }
                }
            }

            return groups;
        }
    }
}