using Microsoft.Extensions.DependencyInjection;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.Services;

namespace Microsoft.ToDo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}