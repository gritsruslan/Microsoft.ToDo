using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.API.Helpers;
using Microsoft.ToDo.Application.Abstraction;
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
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var categoryDto = await categoryService.CreateCategory(
            new CreateCategoryRequest(name), userId, cancellationToken);
        return Created((string?) null, categoryDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var categories = await categoryService.GetAllCategories(userId, cancellationToken);
        return Ok(categories);
    }
}