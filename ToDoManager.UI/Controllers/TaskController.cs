using Microsoft.AspNetCore.Mvc;
using ToDoManager.Application.Services;
using ToDoManager.UI.Models;

namespace ToDoManager.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        await _taskService.AddTask(model.AccountId, model.Name, model.Text, CancellationToken, model.Deadline, model.GroupId);
        return Ok();
    } 
}