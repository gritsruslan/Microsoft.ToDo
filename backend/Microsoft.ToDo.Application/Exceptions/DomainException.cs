namespace Microsoft.ToDo.Application.Exceptions;

public class DomainException(
    string message,
    DomainErrorCode errorCode = DomainErrorCode.BadRequest) : Exception(message);