namespace Microsoft.ToDo.Application.DTOs;

public sealed record TaskResponse(int Id, string Title, DateTimeOffset? DueDate, bool IsCompleted, int CategoryId);