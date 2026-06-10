namespace Microsoft.ToDo.Application.DTOs;

public sealed class TaskResponse(int Id, string Title, DateTimeOffset? DueDate, bool IsCompleted, int CategoryId);