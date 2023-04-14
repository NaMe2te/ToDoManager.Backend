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
        int accountId = Int32.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
        await _taskService.CreateTask(accountId, model.Name, model.Text, CancellationToken, model.Deadline, model.GroupId);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> RemoveTask([FromQuery] int id)
    {
        await _taskService.RemoveTask(id, CancellationToken);
        return Ok();
    }

    [HttpPut("rename")]
    public async Task<ActionResult<TaskDto>> RenameTask([FromBody] RenameTaskModel model)
    {
        TaskDto task = await _taskService.RenameTask(model.TaskId, model.NewName, CancellationToken);
        return Ok(task);
    }

    [HttpPut("add-deadline")]
    public async Task<ActionResult<TaskDto>> AddDeadlineTask([FromBody] AddDeadlineToTaskModel model)
    {
        TaskDto task = await _taskService.AddDeadline(model.TaskId, model.NewDate, CancellationToken);
        return Ok(task);
    }

    [HttpPut("edit-text")]
    public async Task<ActionResult<TaskDto>> EditTaskText([FromBody] EditTaskTextModel model)
    {
        TaskDto task = await _taskService.EditTaskText(model.TaskId, model.NewText, CancellationToken);
        return Ok(task);
    }

    [HttpPut("change-status")]
    public async Task<ActionResult<TaskDto>> Complete([FromBody] ChangeTaskStatusModel model)
    {
        TaskDto task = await _taskService.ChangeStatus(model.TaskId, model.Status, CancellationToken);
        return Ok(task);
    } 

    [HttpGet("get-tasks-without-group")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksWithoutGroup()
    {
        int accountId = Int32.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
        IEnumerable<TaskDto> tasks = await _taskService.GetTasksWithoutGroupByAccount(accountId, CancellationToken);
        return Ok(tasks);
    }
    
    [HttpGet("get-tasks-by-group")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByGroup([FromQuery] int id)
    {
        IEnumerable<TaskDto> tasks = await _taskService.GetTasksByGroup(id, CancellationToken);
        return Ok(tasks);
    }
}