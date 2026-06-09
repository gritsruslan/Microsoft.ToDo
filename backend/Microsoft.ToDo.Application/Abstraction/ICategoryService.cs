using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.Application.Abstraction;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategory(
        CreateCategoryRequest request, 
        string? userId, 
        CancellationToken cancellationToken);
}