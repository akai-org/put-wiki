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
            .HasMaxLength(User.MaxNicknameLength)
            .IsRequired(false);

        builder.HasIndex(x => x.Nickname)
            .IsUnique()
            .HasFilter("\"Nickname\" IS NOT NULL")
            .UseCollation("und-x-icu");
    }
}