using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Map;
public class ProjectMap : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(Project));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

        builder.HasMany(p => p.Tasks)
               .WithOne(t => t.Project)
               .HasForeignKey(t => t.ProjectId);

        builder.HasOne(p => p.User)
               .WithMany(u => u.Projects)
               .HasForeignKey(p => p.UserId)
               .IsRequired();
    }
}
