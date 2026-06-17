using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.Application.Abstraction;

public interface ICategoryService
{
    Task<CategoryResponse> CreateCategory(
        CreateCategoryRequest request, 
        string? userId, 
        CancellationToken cancellationToken);
    
    Task<IEnumerable<CategoryResponse>> GetAllCategories(
        string? userId, CancellationToken cancellationToken);
    
    Task<CategoryResponse> GetCategory(int categoryId, string? userId, CancellationToken cancellationToken);
}