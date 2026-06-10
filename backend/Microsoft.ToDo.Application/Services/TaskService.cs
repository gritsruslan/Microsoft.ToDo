using FluentValidation;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;

namespace Microsoft.ToDo.Application.Services;

internal sealed class TaskService(
    ITaskRepository taskRepository, 
    ICategoryRepository categoryRepository,
    IValidator<CreateTaskRequest> createTaskValidator,
    IValidator<SearchTasksRequest> searchTasksValidator,
    IValidator<UpdateTaskRequest> updateTaskValidator) : ITaskService
{
    public async Task<TaskResponse> CreateTask(
        CreateTaskRequest request, string? userId, CancellationToken cancellationToken)
    {
        await createTaskValidator.ValidateAndThrowAsync(request, cancellationToken);
        
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

    public async Task<PagedData<TaskReadModel>> SearchTasks(
        SearchTasksRequest request, 
        string? userId, 
        CancellationToken cancellationToken)
    {
        await searchTasksValidator.ValidateAndThrowAsync(request, cancellationToken);
        
        var (searchQuery, categoryId, page, pageSize) = request;
        
        if (userId is null)
        {
            throw new UnauthorizedException();
        }

        // "categoryId is not null" is not working properly for some reason
        if (categoryId is { } categoryIdNotNull)
        {
            var category = await categoryRepository.GetById(categoryIdNotNull, cancellationToken);
            if (category is null)
            {
                throw new CategoryNotFoundException(categoryIdNotNull);
            }
            if (category.UserId != userId)
            {
                throw new ForbiddenException();
            }
        }
        
        int skip = (page - 1) * pageSize;
        int take = pageSize;
        
        var (tasks, totalCount) = await taskRepository.Search(
            searchQuery, categoryId, userId, skip, take, cancellationToken);

        var taskReadModels = tasks.Select(t =>
            new TaskReadModel(t.Id, t.Title, t.DueDate, t.CategoryId, t.Category.Name));

        return new PagedData<TaskReadModel>(taskReadModels, totalCount, page, pageSize);
    }

    public async Task UpdateTask(
        UpdateTaskRequest request, 
        string? userId, 
        CancellationToken cancellationToken)
    {
        await updateTaskValidator.ValidateAndThrowAsync(request, cancellationToken);
        
        var (taskId, title, dueDate, isCompleted) = request;
        if (userId is null)
        {
            throw new UnauthorizedException();
        }

        var task = await taskRepository.GetById(taskId, cancellationToken);
        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }

        await taskRepository.Update(taskId, title, dueDate, isCompleted, cancellationToken);
    }

    public async Task DeleteTask(int taskId, string? userId, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetById(taskId, cancellationToken);

        if (task is null)
        {
            throw new TaskNotFoundException(taskId);
        }
        
        if (task.UserId != userId)
        {
            throw new ForbiddenException();
        }
        
        await taskRepository.Delete(taskId, cancellationToken);
    }
}