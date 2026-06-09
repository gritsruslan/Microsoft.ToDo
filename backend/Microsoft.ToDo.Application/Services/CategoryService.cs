using FluentValidation;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;

namespace Microsoft.ToDo.Application.Services;

internal sealed class CategoryService(ICategoryRepository repository, 
    IValidator<CreateCategoryRequest> validator): ICategoryService
{
    public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest request, string? userId, CancellationToken cancellationToken)
    {
        if (userId is null)
        {
            throw new UnauthorizedException();
        }
        
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var category = await repository.Create(request.Name, userId, cancellationToken);

        return new CategoryResponse(category.Id, category.Name);
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategories(
        string? userId, CancellationToken cancellationToken)
    {
        if (userId is null)
        {
            throw new UnauthorizedException();
        }

        var categories = await repository.GetAllByUser(userId, cancellationToken);
        
        return categories.Select(c => new CategoryResponse(c.Id, c.Name));
    }
}