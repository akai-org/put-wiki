using Domain.AcademicTeachers;
using Domain.Users;

using Infrastructure.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public DbSet<AcademicTeacher>? AcademicTeachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
        new AcademicTeacherEntityTypeConfiguration().Configure(modelBuilder.Entity<AcademicTeacher>());
    }
}