using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Map;
public class TaskItemMap : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable(nameof(TaskItem));
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Description)
               .HasMaxLength(1000);

        builder.Property(t => t.Status)
               .IsRequired();

        builder.Property(t => t.Priority)
               .IsRequired();

        builder.Property(t => t.DueDate)
               .IsRequired(false);

        builder.Property(c => c.UserId)
               .IsRequired();

        builder.HasOne(t => t.Project)
               .WithMany(p => p.Tasks)
               .HasForeignKey(t => t.ProjectId);

        builder.HasMany(t => t.Comments)
               .WithOne()
               .HasForeignKey(c => c.TaskItemId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.ChangeLogs)
               .WithOne()
               .HasForeignKey(c => c.TaskItemId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
