using MySql.Data.MySqlClient;
using ToDoManager.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.DataAccess.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public AccountRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task CreateAsync(Account model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var createQuery = $"insert into {connection.Database}.accounts(username, password)" +
                              $" values ({model.Username}, {model.Password});";
            await using (var command = new MySqlCommand(createQuery, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task UpdateAsync(Account model, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            var updateQuery = $"update {connection.Database}.accounts"
                              + $" set username = '{model.Username}', password = '{model.Password}'"
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
            var deleteQuery = $"delete from {connection.Database}.accounts where id = {id};";
            await using (var command = new MySqlCommand(deleteQuery, connection))
                await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    public async Task<Account> GetModelAsync(int id, CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            Account? account = null;
            await connection.OpenAsync(cancellationToken);
            var getQuery = $"select * from {connection.Database}.accounts where id = {id};";
            await using (var command = new MySqlCommand(getQuery, connection))
            {
                await using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accountId = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        account = new Account(accountId, username, password);
                    }
                }
            }

            return account ?? throw new Exception();
        }
    }

    public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken cancellationToken)
    {
        await using (var connection = new MySqlConnection(_connectionString))
        {
            var accounts = new List<Account>();
            await connection.OpenAsync(cancellationToken);
            var getQuery = $"select * from {connection.Database}.accounts;";
            using (var command = new MySqlCommand(getQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var accountId = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        accounts.Add(new Account(accountId, username, password));
                    }
                }
            }

            return accounts;
        }
    }
}