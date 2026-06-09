using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.API.Helpers;
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
        AuthTokenHelper.StoreAccessToken(HttpContext, token);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var token = await service.Login(loginRequest, cancellationToken);
        AuthTokenHelper.StoreAccessToken(HttpContext, token);
        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        AuthTokenHelper.RemoveAccessToken(HttpContext);
        return NoContent();
    }
}