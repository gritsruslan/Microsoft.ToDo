using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.Auth;
using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.API.Controllers;

[ApiController]
[Route("categories")]
public sealed class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        string? userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
        var categoryDto = await categoryService.CreateCategory(new CreateCategoryRequest(name), userId, cancellationToken);
        return Created((string?) null, categoryDto);
    }
}