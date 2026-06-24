using Microsoft.Extensions.Options;

namespace Microsoft.ToDo.API.Extensions;

internal static class CorsExtensions
{
    public const string FrontendPolicyName = "Frontend";
    
    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services, 
        FrontendOptions frontendOptions)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                policy
                    .WithOrigins(frontendOptions.BaseUrls)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }
}