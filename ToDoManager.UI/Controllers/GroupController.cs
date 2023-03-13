﻿using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Services;

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
    
    [HttpPost("add-new-group")]
    public async Task<ActionResult> AddNewGroup([FromQuery] string groupName)
    {
        await _groupService.AddGroup(groupName, CancellationToken);
        return Ok();
    }

    [HttpGet("get-group")]
    public async Task<ActionResult<GroupDto>> GetGroup([FromQuery] int id)
    {
        GroupDto groupDto = await _groupService.GetGroup(id, CancellationToken);
        return Ok(groupDto);
    }

    [HttpPut("rename-group")]
    public async Task<ActionResult<GroupDto>> RenameGroup([FromQuery] int id, string newName)
    {
        GroupDto groupDto = await _groupService.RenameGroup(id, newName, CancellationToken);
        return Ok(groupDto);
    }

}