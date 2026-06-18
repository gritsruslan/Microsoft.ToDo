using Microsoft.EntityFrameworkCore;
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

    public async Task<(IEnumerable<TaskItem> items, int totalCount)> Search(
        string? searchQuery,
        int? categoryId,
        string userId,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        var query = dbContext.TaskItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            query = query.Where(t => EF.Functions.Like(t.Title, $"%{searchQuery}%", @"\"));
        }

        if (categoryId is not null)
        {
            query = query.Where(t => t.CategoryId == categoryId);
        }
        
        query = query.Where(t => t.UserId == userId);
        
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Include(t => t.Category)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<TaskItem?> GetById(int taskId, CancellationToken cancellationToken) => 
        dbContext.TaskItems.Where(t => t.Id == taskId).FirstOrDefaultAsync(cancellationToken);

    public Task Update(
        int taskId, 
        string title, 
        DateTimeOffset? dueDate, 
        bool isCompleted,
        int categoryId,
        CancellationToken cancellationToken)
    {
        return dbContext.TaskItems.Where(t => t.Id == taskId)
            .ExecuteUpdateAsync(s =>
            s.SetProperty(t => t.Title, title)
                .SetProperty(t => t.DueDate, dueDate)
                .SetProperty(t => t.IsCompleted, isCompleted)
                .SetProperty(t => t.CategoryId, categoryId), cancellationToken);
    }

    public Task Delete(int taskId, CancellationToken cancellationToken) => 
        dbContext.TaskItems.Where(t => t.Id == taskId).ExecuteDeleteAsync(cancellationToken);
}