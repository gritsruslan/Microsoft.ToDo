namespace Microsoft.ToDo.Application.Exceptions;

public sealed class CategoryNotFoundException(int categoryId) : 
    DomainException($"Category with id {categoryId} not found");