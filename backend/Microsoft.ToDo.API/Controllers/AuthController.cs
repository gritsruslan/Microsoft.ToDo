using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.API.Helpers;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.API.Controllers;

[ApiController]
[Route("api/auth")]
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

    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = AuthTokenHelper.GetUserIdClaim(HttpContext);
        var response = await service.GetMe(userId, cancellationToken);
        return Ok(response);
    }
}