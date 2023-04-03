using ToDoManager.Application.Dto;
using ToDoManager.DataAccess.Models;

namespace ToDoManager.Application.Mapping;

public static class AccountMapping
{
    public static AccountDto AsDto(this Account account) =>
        new AccountDto(account.Id, account.Username, account.Password);
}