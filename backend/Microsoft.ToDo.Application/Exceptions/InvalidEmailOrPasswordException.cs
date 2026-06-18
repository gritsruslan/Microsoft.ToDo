namespace Microsoft.ToDo.Application.Exceptions;

public sealed class InvalidEmailOrPasswordException() : 
    DomainException("Invalid email or password", DomainErrorCode.Unauthorized);