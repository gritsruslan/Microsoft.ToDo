namespace Microsoft.ToDo.Application.Exceptions;

public sealed class TaskNotFoundException(int taskId) : 
    DomainException($"Category with id {taskId} not found");