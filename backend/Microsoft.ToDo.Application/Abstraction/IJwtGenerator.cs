using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Abstraction;

public interface IJwtGenerator
{
    string GenerateAccessToken(ApplicationUser user);
}