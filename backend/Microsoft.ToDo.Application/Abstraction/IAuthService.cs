using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.Application.Abstraction;

public interface IAuthService
{
    Task<string> Register(RegisterRequest request, CancellationToken cancellationToken);
    
    Task<string> Login(LoginRequest request, CancellationToken cancellationToken);
}