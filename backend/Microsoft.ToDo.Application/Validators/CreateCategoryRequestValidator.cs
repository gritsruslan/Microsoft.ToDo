using FluentValidation;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Domain.Constants;

namespace Microsoft.ToDo.Application.Validators;

internal sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name can't be empty")
            .MinimumLength(CategoryConstants.NameMinLength)
            .WithMessage($"Name must be at least {CategoryConstants.NameMinLength} characters long.")
            .MaximumLength(CategoryConstants.NameMaxLength)
            .WithMessage($"Name must be at most {CategoryConstants.NameMaxLength} characters long.");
    }
}