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
}