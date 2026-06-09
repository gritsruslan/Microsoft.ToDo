using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Application.Abstraction;

public interface ICategoryRepository
{
    Task<Category> Create(string name, string userId, CancellationToken cancellationToken);
}