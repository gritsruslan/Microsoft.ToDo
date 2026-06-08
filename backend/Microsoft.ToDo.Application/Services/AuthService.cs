using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Services;

internal sealed class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    public async Task<string> Register(
        RegisterRequest request, 
        CancellationToken cancellationToken)
    {
        var (email, password) = request;
        
        var result = await userManager.CreateAsync(new ApplicationUser
        {
            UserName = email,
            Email = email
        }, password);
        
        if (!result.Succeeded)
        {
            throw new DomainException(result.Errors.First().Description);
        }

        return string.Empty;
    }

    public async Task<string> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var (email, password) = request;
        
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new InvalidEmailOrPasswordException();
        }
        
        var isValidPassword = await userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
        {
            throw new InvalidEmailOrPasswordException();
        }

        return string.Empty;
    }
}