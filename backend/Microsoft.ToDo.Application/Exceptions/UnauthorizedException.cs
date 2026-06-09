namespace Microsoft.ToDo.Application.Exceptions;

public class UnauthorizedException() : DomainException(null, DomainErrorCode.Unauthorized);