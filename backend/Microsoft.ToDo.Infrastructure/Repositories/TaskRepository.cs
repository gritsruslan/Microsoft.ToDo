using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Infrastructure.Repositories;

internal sealed class TaskRepository(ToDoDbContext dbContext) : ITaskRepository
{
    public async Task<TaskItem> Create(
        string title, 
        DateTimeOffset? dueDate, 
        int categoryId, 
        string userId, 
        CancellationToken cancellationToken)
    {
        var task = await dbContext.TaskItems.AddAsync(new TaskItem
        {
            Title = title,
            CategoryId = categoryId,
            UserId = userId,
            DueDate = dueDate,
            User = null!,
            Category = null!,
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return task.Entity;
    }
}