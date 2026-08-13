using Domain.AcademicTeachers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration;

public class AcademicTeacherEntityTypeConfiguration : IEntityTypeConfiguration<AcademicTeacher>
{
    public void Configure(EntityTypeBuilder<AcademicTeacher> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UsosId).IsRequired();
        builder.HasIndex(x => x.UsosId).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);

        builder.Property(x => x.PhoneNumber).HasMaxLength(30);
        builder.Property(x => x.PhotoUrl).HasMaxLength(2048);
        builder.Property(x => x.WebsiteUrl).HasMaxLength(2048);

        builder.Property(x => x.Description).HasMaxLength(2500);

        builder.Property(x => x.Degrees).HasField("_degrees").UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
}