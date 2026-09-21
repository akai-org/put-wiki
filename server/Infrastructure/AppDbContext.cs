using Domain.Users;

using Infrastructure.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<RefreshSession> RefreshSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
        new RefreshSessionEntityTypeConfiguration().Configure(modelBuilder.Entity<RefreshSession>());
    }
}