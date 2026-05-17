using Domain;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public required DbSet<Opinion> Opinions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}