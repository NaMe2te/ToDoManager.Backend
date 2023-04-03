using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Exceptions.AlreadyExist;
using ToDoManager.Application.Services;
using ToDoManager.DataAccess.Exceptions.InvalidDataException;
using ToDoManager.UI.Models.Accounts;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromQuery] AccountModel accountModel)
    {
        try
        {
            await _accountService.Register(accountModel.Username, accountModel.Password, CancellationToken);
            return Ok();
        }
        catch (UsernameAlreadyExistException e)
        {
            return Conflict(e);
        }
    }
    

    [HttpPost("login")]
    public async Task<ActionResult<AccountDto>> Login([FromQuery] AccountModel accountModel)
    {
        try
        {
            AccountDto accountDto = await _accountService.Login(accountModel.Username, accountModel.Password, CancellationToken);
            return Ok(accountDto);
        }
        catch (InvalidUsernameOrPasswordException e) 
        {
            return NotFound(e);
        }
    }
}