namespace Microsoft.ToDo.Application.DTOs;

public sealed record SearchTasksRequest(string? SearchQuery, int? CategoryId, int Page = 1, int PageSize = 20);