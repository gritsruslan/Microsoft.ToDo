using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Services;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtGenerator jwtGenerator,
    IValidator<LoginRequest> loginValidator,
    IValidator<RegisterRequest> registerValidator) : IAuthService
{
    public async Task<string> Register(
        RegisterRequest request, 
        CancellationToken cancellationToken)
    {
        await registerValidator.ValidateAndThrowAsync(request, cancellationToken);
        
        var (email, password) = request;
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };
        
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new DomainException(result.Errors.First().Description);
        }

        return jwtGenerator.GenerateAccessToken(user);
    }

    public async Task<string> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        await loginValidator.ValidateAndThrowAsync(request, cancellationToken);
        
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

        return jwtGenerator.GenerateAccessToken(user);
    }

    public async Task<GetMeResponse> GetMe(string? userId, CancellationToken cancellationToken)
    {
        if (userId is null)
        {
            throw new UnauthorizedException();
        }
        
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedException();
        }
        
        return new GetMeResponse(user.Id, user.Email ?? throw new InvalidOperationException());
    }
}