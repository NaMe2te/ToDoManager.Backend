using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Services;
using ToDoManager.UI.Models.Groups;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost("create")]
    public async Task<ActionResult> CreateGroup([FromBody] CreateGroupModel model)
    {
        int accountId = Int32.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
        await _groupService.AddGroup(accountId, model.GroupName, CancellationToken);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> RemoveGroup([FromQuery] int id)
    {
        await _groupService.RemoveGroup(id, CancellationToken);
        return Ok();
    }

    [HttpPut("rename")]
    public async Task<ActionResult<GroupDto>> RenameGroup([FromBody] RenameGroupModel model)
    {
        GroupDto groupDto = await _groupService.RenameGroup(model.Id, model.NewName, CancellationToken);
        return Ok(groupDto);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetAllGroupsByAccount()
    {
        int accountId = Int32.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
        IEnumerable<GroupDto> groups = await _groupService.GetAllByAccount(accountId, CancellationToken);
        return Ok(groups);
    }
}