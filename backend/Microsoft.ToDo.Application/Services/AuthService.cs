using FluentValidation;
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
}