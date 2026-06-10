using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Abstraction;

public interface ITaskRepository
{
    Task<TaskItem> Create(
        string title, 
        DateTimeOffset? dueDate, 
        int categoryId, 
        string userId, 
        CancellationToken cancellationToken);
}