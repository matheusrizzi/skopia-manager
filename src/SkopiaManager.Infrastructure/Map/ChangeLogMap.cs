using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Map
{
    public class ChangeLogMap : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            builder.ToTable(nameof(ChangeLog));
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(c => c.UserId)
                   .IsRequired();

            builder.Property(c => c.ModifiedAt)
                   .IsRequired();
        }
    }
}
