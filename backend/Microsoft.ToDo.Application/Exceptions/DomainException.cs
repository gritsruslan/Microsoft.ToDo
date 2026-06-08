namespace Microsoft.ToDo.Application.Exceptions;

public enum DomainErrorCode
{
    BadRequest = 400,
}

public class DomainException(
    string message,
    DomainErrorCode errorCode = DomainErrorCode.BadRequest) : Exception(message);