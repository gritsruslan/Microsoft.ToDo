using Microsoft.EntityFrameworkCore;
using Microsoft.ToDo.Infrastructure;

namespace Microsoft.ToDo.API.Extensions;

internal static class DatabaseMigrationExtensions
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}