using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.HashedUsosId).IsRequired();
        builder.HasIndex(x => x.HashedUsosId).IsUnique();

        builder.Property(x => x.Nickname)
            .HasConversion(
                n => n != null ? n.Value : null,
                v => v != null ? Nickname.Create(v).Value : null)
            .HasMaxLength(Nickname.MaxLength)
            .IsRequired(false)
            .HasColumnType("citext");

        builder.HasIndex(x => x.Nickname)
            .IsUnique()
            .HasFilter("\"Nickname\" IS NOT NULL");
    }
}