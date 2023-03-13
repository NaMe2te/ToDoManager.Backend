using System.Net;
using MySql.Data.MySqlClient;
using Group = ToDoManager.DataAccess.Models.Group;

namespace ToDoManager.DataAccess.Repositories.Implementations;

public class GroupRepository : IGroupRepository
{
    private readonly string _connectionString;
    
    public GroupRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task CreateAsync(Group model, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            string createQuery = $"insert into {connection.Database}.groups(name)"
                                 + $" value ('{model.Name}');";
            using (var command = new MySqlCommand(createQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task UpdateAsync(Group model, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            string updateQuery = $"update {connection.Database}.groups"
                                 + $" set name = '{model.Name}'"
                                 + $" where id = {model.Id};";
            using (var command = new MySqlCommand(updateQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            string deleteQuery = $"delete from {connection.Database}.groups where id = {id};";
            using (var command = new MySqlCommand(deleteQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task<Group> GetModelAsync(int id, CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            Group? group = null;
            await connection.OpenAsync(cancellationToken);
            string getQuery = $"select * from {connection.Database}.groups where id = {id};";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        group = new Group(groupId, name);
                    }
                }
            }

            return group ?? throw new Exception();
        }
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var groups = new List<Group>();
            await connection.OpenAsync(cancellationToken);
            string getQuery = $"select * from {connection.Database}.groups;";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        groups.Add(new Group(groupId, name));
                    }
                }
            }

            return groups;
        }
    }
}