using Microsoft.EntityFrameworkCore;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Infrastructure.Repositories;

internal sealed class CategoryRepository(ToDoDbContext dbContext) : ICategoryRepository
{
    public async Task<Category> Create(string name, string userId, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.AddAsync(new Category
        {
            Name = name,
            UserId = userId,
            User = null!
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return category.Entity;
    }

    public Task<Category?> GetById(int taskId, CancellationToken cancellationToken) => 
        dbContext.Categories.Where(c => c.Id == taskId).FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Category>> GetAllByUser(
        string userId, CancellationToken cancellationToken) =>
        await dbContext.Categories
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
}