using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Abstraction;

public interface IAuthService
{
    Task<string> Register(RegisterRequest request, CancellationToken cancellationToken);
    
    Task<string> Login(LoginRequest request, CancellationToken cancellationToken);
    
    Task<GetMeResponse> GetMe(string? userId, CancellationToken cancellationToken);
}