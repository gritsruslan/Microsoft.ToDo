namespace Microsoft.ToDo.Application.Exceptions;

public class TaskNotFoundException(int taskId) : 
    DomainException($"Category with id {taskId} not found");