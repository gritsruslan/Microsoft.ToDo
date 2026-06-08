namespace Microsoft.ToDo.Application.Exceptions;

public class InvalidEmailOrPasswordException() : 
    DomainException("Invalid email or password", DomainErrorCode.Unauthorized);