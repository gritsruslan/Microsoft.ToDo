namespace Microsoft.ToDo.Domain.Models;

public sealed class Category
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string UserId { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public required ApplicationUser User { get; set; }
    
    public ICollection<TaskItem> TaskItems { get; set; } = [];
}