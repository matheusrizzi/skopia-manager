using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Map;

public class CommentMap : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(nameof(Comment));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Message)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(c => c.CreatedAt)
               .IsRequired();

        builder.Property(c => c.UserId)
               .IsRequired();
    }
}
