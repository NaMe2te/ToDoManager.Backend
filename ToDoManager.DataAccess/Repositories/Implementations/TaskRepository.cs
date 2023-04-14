using MySql.Data.MySqlClient;
using ToDoManager.DataAccess.Exceptions.NotFound;

namespace ToDoManager.DataAccess.Repositories.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task CreateAsync(Models.Task model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var deadline = model.Deadline is not null ? $"'{model.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";
            var groupId = model.GroupId is not null ? $"{model.GroupId}" : "NULL";
            var createQuery = $"insert into {connection.Database}.tasks(name, text, is_completed, deadline, group_id, account_id)"
                              + $" values ('{model.Name}', '{model.Text}', {model.IsCompleted}, {deadline}, {groupId}, {model.AccountId});";
            await using (var command = new MySqlCommand(createQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task UpdateAsync(Models.Task model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            string deadline = model.Deadline is not null ? $"'{model.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";
            var groupId = model.GroupId is not null ? $"{model.GroupId}" : "NULL";
            var updateQuery = $"update {connection.Database}.tasks"
                              + $" set name = '{model.Name}', text = '{model.Text}', is_completed = {model.IsCompleted}, deadline = {deadline}, group_id = {groupId}"
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
            var deleteQuery = $"delete from {connection.Database}.tasks"
                              + $" where id = {id};";
            await using (var command = new MySqlCommand(deleteQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task<Models.Task> GetModelAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            Models.Task? foundTask = null;
            var getQuery = $"select * from {connection.Database}.tasks"
                           + $" where id = {id}";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(1);
                        string text = reader.GetString(2);
                        bool isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        int accountId = reader.GetInt32(6);
                        foundTask = new Models.Task(id, name, text, accountId, deadline, groupId, isCompleted);
                    }
                    
                    return foundTask ?? throw new EntityNotFoundException<Models.Task>(id);
                }
            }
        }
    }

    public async Task<IEnumerable<Models.Task>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            List<Models.Task> tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string text = reader.GetString(2);
                        bool isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        int accountId = reader.GetInt32(6);
                        tasks.Add(new Models.Task(id, name, text, accountId, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }
    
    public async Task<IEnumerable<Models.Task>> GetTasksWithoutGroupByAccount(int accountId, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks where account_id = {accountId} and group_id is NULL and is_completed = 0";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string text = reader.GetString(2);
                        bool isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        tasks.Add(new Models.Task(id, name, text, accountId, deadline, null, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }

    public async Task<IEnumerable<Models.Task>> GetTasksByGroup(int groupId, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks where group_id = {groupId} and is_completed = 0";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string text = reader.GetString(2);
                        bool isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int accountId = reader.GetInt32(6);
                        tasks.Add(new Models.Task(id, name, text, accountId, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }
}