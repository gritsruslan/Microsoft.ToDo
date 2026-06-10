namespace Microsoft.ToDo.Application.DTOs;

public sealed record UpdateTaskRequest(string Title, DateTimeOffset? DueDate, bool IsCompleted);