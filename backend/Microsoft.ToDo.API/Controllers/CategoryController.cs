using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.API.Helpers;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.API.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var categoryDto = await service.CreateCategory(
            new CreateCategoryRequest(name), userId, cancellationToken);
        return Created(string.Empty, categoryDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var categories = await service.GetAllCategories(userId, cancellationToken);
        return Ok(categories);
    }

    [HttpGet("{categoryId::int}")]
    public async Task<IActionResult> Get(
        [FromRoute] int categoryId, 
        CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var category = await service.GetCategory(categoryId, userId, cancellationToken);
        return Ok(category);
    }
}