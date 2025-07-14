using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Map;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Role)
               .IsRequired()
               .HasDefaultValue("Desenvolvedor")
               .HasMaxLength(40);

        builder.HasData(
                        new User { Id = 1, Name = "Gerente", Role = "Gerente" },
                        new User { Id = 2, Name = "Desenvolvedor", Role = "Desenvolvedor" }
                       );
    }
}
