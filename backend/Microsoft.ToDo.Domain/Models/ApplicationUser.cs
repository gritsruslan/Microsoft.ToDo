using Microsoft.AspNetCore.Identity;

namespace Microsoft.ToDo.Domain.Models;

public sealed class ApplicationUser : IdentityUser
{
    public override string? UserName { get; set; }
    
    public override string? Email { get; set; }
    
    public ICollection<Category> Categories { get; set; } = [];
    
    public ICollection<TaskItem> TaskItems { get; set; } = [];
}