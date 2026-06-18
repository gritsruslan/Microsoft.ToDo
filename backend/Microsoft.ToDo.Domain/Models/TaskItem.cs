namespace Microsoft.ToDo.Domain.Models;

// The name TaskItem was chosen to avoid a conflict with the Task class 
public sealed class TaskItem
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? DueDate { get; set; }

    public required string UserId { get; set; }
    
    public required ApplicationUser User { get; set; }
    
    public int CategoryId { get; set; }
    
    public required Category Category { get; set; }
}