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

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName != null)
            {
                entity.SetTableName(tableName.ToSnakeCase());
            }

            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                property.SetColumnName(columnName.ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (keyName != null)
                {
                    key.SetName(keyName.ToSnakeCase());
                }
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (fkName != null)
                {
                    fk.SetConstraintName(fkName.ToSnakeCase());
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (indexName != null)
                {
                    index.SetDatabaseName(indexName.ToSnakeCase());
                }
            }
        }

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
        new AcademicTeacherEntityTypeConfiguration().Configure(modelBuilder.Entity<AcademicTeacher>());
    }
}