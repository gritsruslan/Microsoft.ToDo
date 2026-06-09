namespace Microsoft.ToDo.Application.Exceptions;

public class UnauthorizedException() : DomainException("Unauthorized", DomainErrorCode.Unauthorized);