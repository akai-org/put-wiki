using Domain.Users;

using Infrastructure.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("citext");

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
    }
}