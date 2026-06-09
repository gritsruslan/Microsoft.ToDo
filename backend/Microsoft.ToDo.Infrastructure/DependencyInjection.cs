using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Infrastructure.Repositories;

namespace Microsoft.ToDo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextPool<ToDoDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("MsSql"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}