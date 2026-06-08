using Microsoft.AspNetCore.Mvc;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;

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
        //TODO generate and save JWT
        await service.Register(registerRequest, cancellationToken);
        return Ok();
    }
}