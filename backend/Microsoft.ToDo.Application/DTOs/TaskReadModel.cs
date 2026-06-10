namespace Microsoft.ToDo.Application.DTOs;

public sealed record TaskReadModel(
    int Id, 
    string Title, 
    DateTimeOffset? DueDate, 
    int CategoryId, 
    string CategoryName);