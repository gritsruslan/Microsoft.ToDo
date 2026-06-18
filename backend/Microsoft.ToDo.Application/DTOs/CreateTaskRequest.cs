namespace Microsoft.ToDo.Application.DTOs;

public sealed record CreateTaskRequest(string Title, int CategoryId, DateTimeOffset? DueDate);