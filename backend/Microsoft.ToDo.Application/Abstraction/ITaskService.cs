using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.Application.Abstraction;

public interface ITaskService
{
    Task<TaskResponse> CreateTask(
        CreateTaskRequest request, string? userId, CancellationToken cancellationToken);

    Task<PagedData<TaskReadModel>> SearchTasks(
        SearchTasksRequest request, 
        string? userId,
        CancellationToken cancellationToken);
    
    Task UpdateTask(int taskId, UpdateTaskRequest request, string? userId, CancellationToken cancellationToken);
    
    Task DeleteTask(int taskId, string? userId, CancellationToken cancellationToken);
}