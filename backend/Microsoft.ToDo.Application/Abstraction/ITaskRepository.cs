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
    
    Task<(IEnumerable<TaskItem> items, int totalCount)> Search(
        string? searchQuery, 
        int? categoryId,
        string userId,
        int skip, 
        int take, 
        CancellationToken cancellationToken);
    
    Task<TaskItem?> GetById(int id, CancellationToken cancellationToken);

    Task Update(int id, string title, DateTimeOffset? dueDate, bool isCompleted,
        CancellationToken cancellationToken);
    
    Task Delete(int id, CancellationToken cancellationToken);
}