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
}