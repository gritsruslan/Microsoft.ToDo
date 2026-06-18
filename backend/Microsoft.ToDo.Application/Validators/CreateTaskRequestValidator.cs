using FluentValidation;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Domain.Constants;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Validators;

internal sealed class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage("Title can't be empty")
            .MinimumLength(TaskItemConstants.TitleMinLength)
            .WithMessage($"Title must be at least {TaskItemConstants.TitleMinLength} characters long.")
            .MaximumLength(TaskItemConstants.TitleMaxLength)
            .WithMessage($"Title must be at most {TaskItemConstants.TitleMaxLength} characters long.");
    }
}