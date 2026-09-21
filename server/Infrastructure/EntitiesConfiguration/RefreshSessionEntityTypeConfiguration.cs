using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration;

public class RefreshSessionEntityTypeConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TokenHash).IsRequired();
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany(x => x.RefreshSessions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}