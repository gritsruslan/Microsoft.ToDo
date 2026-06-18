using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.Domain.Constants;

namespace Microsoft.ToDo.API.Extensions;

internal static class IdentityExtensions
{
    public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
    {
        return services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = SecurityConstants.PasswordMinLength;
        });
    }
}