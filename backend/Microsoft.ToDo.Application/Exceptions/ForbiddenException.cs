namespace Microsoft.ToDo.Application.Exceptions;

public class ForbiddenException(): DomainException(null, DomainErrorCode.Forbidden);