using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.Auth;
using Microsoft.ToDo.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Microsoft.ToDo.API.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        var token = await service.Register(registerRequest, cancellationToken);
        this.PutAccessToken(token);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var token = await service.Login(loginRequest, cancellationToken);
        this.PutAccessToken(token);
        return Ok();
    }

    [SwaggerIgnore]
    private void PutAccessToken(string accessToken)
    {
        Response.Cookies.Append(JwtCookieNames.AccessToken, accessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
            });
    }
}