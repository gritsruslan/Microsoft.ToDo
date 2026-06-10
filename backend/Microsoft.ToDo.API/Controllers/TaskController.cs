using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.API.Helpers;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.API.Controllers;

[ApiController]
[Route("tasks")]
public sealed class TaskController(ITaskService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var task = await service.CreateTask(request, userId, cancellationToken);
        return Created((string?) null, task);
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] SearchTasksRequest request, CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var tasks = await service.SearchTasks(request, userId, cancellationToken);
        return Ok(tasks);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTaskRequest(
        [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        await service.UpdateTask(request, userId, cancellationToken);
        return Ok();
    }
}