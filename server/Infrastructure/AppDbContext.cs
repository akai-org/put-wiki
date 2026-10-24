using Application.Abstractions;

using Domain.AcademicTeachers;
using Domain.Users;

using Infrastructure.Auth;
using Infrastructure.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<AcademicTeacher> AcademicTeachers { get; set; }
    public virtual required DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
        new AcademicTeacherEntityTypeConfiguration().Configure(modelBuilder.Entity<AcademicTeacher>());
        new RefreshTokenEntityTypeConfiguration().Configure(modelBuilder.Entity<RefreshToken>());
    }
}