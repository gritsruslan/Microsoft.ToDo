namespace Microsoft.ToDo.Application.DTOs;

public sealed record UpdateTaskRequest(int Id, string Title, DateTimeOffset? DueDate, bool IsCompleted);