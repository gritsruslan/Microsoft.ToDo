using FluentValidation;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;

namespace Microsoft.ToDo.Application.Services;

internal sealed class TaskService(
    ITaskRepository taskRepository, 
    ICategoryRepository categoryRepository,
    IValidator<CreateTaskRequest> validator) : ITaskService
{
    public async Task<TaskResponse> CreateTask(
        CreateTaskRequest request, string? userId, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var (title, categoryId, dueDate) = request;

        var category = await categoryRepository.GetById(categoryId, cancellationToken);
        if (category is null)
        {
            throw new CategoryNotFoundException(categoryId);
        }

        if (category.UserId != userId)
        {
            throw new ForbiddenException();
        }

        var task = await taskRepository.Create(title, dueDate, categoryId, userId, cancellationToken);
        return new TaskResponse(task.Id, task.Title, task.CategoryId);
    }
}