namespace Microsoft.ToDo.Application.DTOs;

public sealed record TaskReadModel(
    int Id, 
    string Title, 
    DateTimeOffset? DueDate,
    bool IsCompleted,
    int CategoryId, 
    string CategoryName);