using FluentValidation;
using Microsoft.ToDo.Application.DTOs;

namespace Microsoft.ToDo.Application.Validators;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email can't be empty")
            .EmailAddress()
            .WithMessage("Email is not valid");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password can't be empty");
    }
}