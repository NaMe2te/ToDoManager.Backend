using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Exceptions.AlreadyExist;
using ToDoManager.Application.Exceptions.InvalidDataException;
using ToDoManager.Application.Services;
using ToDoManager.UI.Models.Accounts;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;

    public AccountController(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }

    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("register")]
    public async Task<ActionResult<object>> Register([FromBody] AccountModel accountModel)
    {
        try
        {
            await _accountService.Register(accountModel.Username, accountModel.Password, CancellationToken);
            return await Login(accountModel);
        }
        catch (UsernameAlreadyExistException e)
        {
            return Conflict(e.Message);
        }
    }
    

    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] AccountModel accountModel)
    {
        try
        {
            AccountDto accountDto = await _accountService.Login(accountModel.Username, accountModel.Password, CancellationToken);
            string t = GetToken(accountDto.Id, accountDto.Username);
            return Ok(new {token = t});
        }
        catch (InvalidUsernameOrPasswordException e) 
        {
            return Unauthorized(e.Message);
        }
    }

    private string GetToken(int accountId, string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.Unicode.GetBytes(_configuration.GetSection("jwt").GetValue<string>("SecretKey"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Sid, accountId.ToString()),
                new (ClaimTypes.Name, username)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration.GetSection("jwt").GetValue<string>("Audience"),
            Issuer = _configuration.GetSection("jwt").GetValue<string>("Issuer")
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}