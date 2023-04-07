using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Dto;
using ToDoManager.Application.Services;
using ToDoManager.UI.Models.Tasks;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    public CancellationToken CancellationToken => HttpContext.RequestAborted;

    [HttpPost("create-task")]
    public async Task<ActionResult> CreateTask([FromBody] CreateTaskModel model)
    {
        int accountId = Int32.Parse(HttpContext.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Sid).Value);
        await _taskService.AddTask(accountId, model.Name, model.Text, CancellationToken, model.Deadline, model.GroupId);
        return Ok();
    }

    [HttpGet("get-tasks-without-group")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksWithoutGroup()
    {
        await _taskService.GetTasksWithoutGroupByAccount()
    }
}