using ToDoManager.Application.Dto;

namespace ToDoManager.Application.Services;

public interface IAccountService
{
    Task Register(string username, string password, CancellationToken cancellationToken);
    Task<AccountDto> Login(string username, string password);
}