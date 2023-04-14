using ToDoManager.Application.Dto;
using ToDoManager.Application.Exceptions.AlreadyExist;
using ToDoManager.Application.Exceptions.InvalidDataException;
using ToDoManager.Application.Mapping;
using ToDoManager.DataAccess.Repositories;
using ToDoManager.DataAccess.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDoManager.Application.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task Register(string username, string password, CancellationToken cancellationToken)
    {
        if (await _repository.FindAccountByUsername(username) is not null)
            throw new UsernameAlreadyExistException(username);
        var account = new Account(default, username, password);
        await _repository.CreateAsync(account, cancellationToken);
    }

    public async Task<AccountDto> Login(string username, string password)
    {
        Account? account = await _repository.FindAccountByUsername(username);
        if (account is null || account.Password != password)
            throw new InvalidUsernameOrPasswordException();

        return account.AsDto();
    }
}