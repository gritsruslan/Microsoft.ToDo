using FluentValidation;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Domain.Constants;

namespace Microsoft.ToDo.Application.Validators;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email can't be empty")
            .EmailAddress()
            .WithMessage("Email is not valid");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password can't be empty")
            .MinimumLength(SecurityConstants.PasswordMinLength)
            .WithMessage($"Password must be at least {SecurityConstants.PasswordMinLength} characters long.")
            .MaximumLength(SecurityConstants.PasswordMaxLength)
            .WithMessage($"Password must be at most {SecurityConstants.PasswordMaxLength} characters long.")
            .Must(p => p.Any(char.IsLower))
            .WithMessage("Password must contain at least one lowercase letter.")
            .Must(p => p.Any(char.IsDigit))
            .WithMessage("Password must contain at least one digit.");
    }
}