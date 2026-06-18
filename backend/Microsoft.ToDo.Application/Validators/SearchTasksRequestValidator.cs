using FluentValidation;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Domain.Constants;

namespace Microsoft.ToDo.Application.Validators;

internal sealed class SearchTasksRequestValidator : AbstractValidator<SearchTasksRequest>
{
    public SearchTasksRequestValidator()
    {
        RuleFor(r => r.SearchQuery)
            .NotEmpty()
            .WithMessage("Search query can't be empty")
            .MaximumLength(TaskItemConstants.TitleMaxLength)
            .WithMessage($"Search query must be at most {TaskItemConstants.TitleMaxLength} characters long.")
            .When(r => r.SearchQuery is not null);
        
        RuleFor(r => r.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1");
        
        RuleFor(r => r.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1");
    }
}