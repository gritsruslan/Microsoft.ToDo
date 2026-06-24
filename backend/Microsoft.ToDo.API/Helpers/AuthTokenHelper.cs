using Microsoft.ToDo.Application.Auth;

namespace Microsoft.ToDo.API.Helpers;

internal static class AuthTokenHelper
{
    public static void StoreAccessToken(HttpContext httpContext, string token) =>
        httpContext.Response.Cookies.Append(JwtCookieNames.AccessToken, token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
    
    public static void RemoveAccessToken(HttpContext httpContext) =>
        httpContext.Response.Cookies.Delete(JwtCookieNames.AccessToken);

    public static string? GetUserIdClaim(HttpContext httpContext) => 
        httpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
}