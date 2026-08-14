using Application.Interfaces;

using Domain.AcademicTeachers;
using Domain.Users;

using Infrastructure.EntitiesConfiguration;
using Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<AcademicTeacher> AcademicTeachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
        new AcademicTeacherEntityTypeConfiguration().Configure(modelBuilder.Entity<AcademicTeacher>());
    }
}