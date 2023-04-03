using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Services;
using ToDoManager.UI.Models.Groups;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class GroupController : Controller
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;
    
    [HttpPost("create-group")]
    public async Task<ActionResult> AddNewGroup([FromBody] CreateGroupModel model)
    {
        await _groupService.AddGroup(model.AccountId, model.GroupName, CancellationToken);
        return Ok();
    }

    [HttpGet("get-group")]
    public async Task<ActionResult<GroupDto>> GetGroup([FromQuery] int id)
    {
        GroupDto groupDto = await _groupService.GetGroup(id, CancellationToken);
        return Ok(groupDto);
    }
    
    [HttpGet("get-all-groups")]
    public async Task<ActionResult<IEnumerable<GroupDto>>> GetAllGroups()
    {
        IEnumerable<GroupDto> groups = await _groupService.GetAllGroups(CancellationToken);
        return Ok(groups);
    }

    [HttpPut("rename-group")]
    public async Task<ActionResult<GroupDto>> RenameGroup([FromQuery] int id, string newName)
    {
        GroupDto groupDto = await _groupService.RenameGroup(id, newName, CancellationToken);
        return Ok(groupDto);
    }

}