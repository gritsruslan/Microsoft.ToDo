using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.ToDo.Domain.Constants;
using Microsoft.ToDo.Domain.Models;

namespace Microsoft.ToDo.Infrastructure.Configurations;

internal sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(t => t.Id);

        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()")
            .ValueGeneratedOnAdd();
        
        builder
            .Property(t => t.Title)
            .HasMaxLength(TaskItemConstants.TitleMaxLength);

        builder
            .HasOne(t => t.User)
            .WithMany(u => u.TaskItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(t => t.Category)
            .WithMany(c => c.TaskItems)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}