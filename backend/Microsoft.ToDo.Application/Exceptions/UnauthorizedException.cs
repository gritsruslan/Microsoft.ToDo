namespace Microsoft.ToDo.Application.Exceptions;

public sealed class UnauthorizedException() : DomainException(null, DomainErrorCode.Unauthorized);