using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Infrastructure;

public sealed class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : 
    IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories => Set<Category>();
    
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}