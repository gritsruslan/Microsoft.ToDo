namespace Microsoft.ToDo.Application.Exceptions;

public class CategoryNotFoundException(int categoryId) : 
    DomainException($"Category with id {categoryId} not found");