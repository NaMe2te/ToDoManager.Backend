using System.Net;
using MySql.Data.MySqlClient;
using ToDoManager.DataAccess.Exceptions.NotFound;
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
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var createQuery = $"insert into {connection.Database}.groups(name, account_id)"
                              + $" value ('{model.Name}', {model.AccountId});";
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
                        int accountId = reader.GetInt32(2);
                        group = new Group(groupId, name, accountId);
                    }
                }
            }

            if (group is null) 
                throw new EntityNotFoundException<Group>(id);

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
                        int accountId = reader.GetInt32(2);
                        var group = new Group(groupId, name, accountId);
                        groups.Add(group);
                        
                    }
                }
            }

            return groups;
        }
    }

    public async Task<IEnumerable<Group>> GetAllByAccount(int accountId, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            var groups = new List<Group>();
            await connection.OpenAsync(cancellationToken);
            string getQuery = $"select * from {connection.Database}.groups where account_id = {accountId};";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int groupId = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        var group = new Group(groupId, name, accountId);
                        groups.Add(group);
                    }
                }
            }

            return groups;
        }
    }
}