using MySql.Data.MySqlClient;

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
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var deadline = model.Deadline is not null ? $"'{model.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";
            var groupId = model.GroupId is not null ? $"{model.GroupId}" : "NULL";
            var createQuery = $"insert into {connection.Database}.tasks(name, text, is_completed, deadline, group_id)"
                              + $" values ('{model.Name}', '{model.Text}', {model.IsCompleted}, {deadline}, {groupId});";
            using (var command = new MySqlCommand(createQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task UpdateAsync(Models.Task model, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            string deadline = model.Deadline is not null ? $"'{model.Deadline.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";
            var updateQuery = $"update {connection.Database}.tasks"
                              + $" set name = '{model.Name}', text = '{model.Text}', is_competed = {model.IsCompleted}, deadline = {deadline}, group_id = {model.GroupId}"
                              + $" where id = {model.Id};";
            using (var command = new MySqlCommand(updateQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var deleteQuery = $"delete from {connection.Database}.tasks"
                              + $" where id = {id};";
            using (var command = new MySqlCommand(deleteQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task<Models.Task> GetModelAsync(int id, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            Models.Task? foundTask = null;
            var getQuery = $"select * from {connection.Database}.tasks"
                           + $" where id = {id}";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(1);
                        string text = reader.GetString(2);
                        bool isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        foundTask = new Models.Task(id, name, text, deadline, groupId, isCompleted);
                    }
                    
                    return foundTask ?? throw new Exception("Таска с таким Id не найдена");
                }
            }
        }
    }

    public async Task<IEnumerable<Models.Task>> GetAllAsync(CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            List<Models.Task> tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var text = reader.GetString(2);
                        var isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        tasks.Add(new Models.Task(id, name, text, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }

    public async Task<IEnumerable<Models.Task>> GetTasksWithoutGroup(CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            List<Models.Task> tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks where group_id = null";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var text = reader.GetString(2);
                        var isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        tasks.Add(new Models.Task(id, name, text, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }

    public async Task<IEnumerable<Models.Task>> GetTasksByGroup(int groupId, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            List<Models.Task> tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks where group_id = {groupId}";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var text = reader.GetString(2);
                        var isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        tasks.Add(new Models.Task(id, name, text, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }

    public async Task<IEnumerable<Models.Task>> GetCompletedTasks(CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            List<Models.Task> tasks = new List<Models.Task>();
            var getQuery = $"select * from {connection.Database}.tasks where is_completed = 1";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var text = reader.GetString(2);
                        var isCompleted = reader.GetBoolean(3);
                        DateTime? deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                        int? groupId = reader.IsDBNull(5) ? null : reader.GetInt32(5);
                        tasks.Add(new Models.Task(id, name, text, deadline, groupId, isCompleted));
                    }
                }
            }

            return tasks;
        }
    }
}