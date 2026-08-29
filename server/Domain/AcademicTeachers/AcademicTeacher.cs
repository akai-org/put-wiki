using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.AcademicTeachers;

public class AcademicTeacher
{
    public required Guid Id { get; init; }
    public required string UsosId { get; init; }
    public AcademicTeacherSlug Slug { get; private set; } = null!;

    private List<string> _degrees = null!;
    public IReadOnlyList<string> Degrees => _degrees.AsReadOnly();
    public string Name { get; private set; } = null!;
    public string? PhotoUrl { get; private set; }

    public string Email { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? Description { get; private set; }

    private AcademicTeacher() { }

    public static AcademicTeacher Create(
        string usosId,
        IEnumerable<string> degrees,
        string name,
        string email,
        string? photoUrl = null,
        string? phoneNumber = null,
        string? websiteUrl = null,
        string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(usosId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        ArgumentNullException.ThrowIfNull(degrees);

        var degreeList = degrees.Where(degree => !string.IsNullOrWhiteSpace(degree)).ToList();
        if (degreeList.Count == 0)
            throw new ArgumentException("Degrees collection must contain at least one non-empty value.", nameof(degrees));

        var academicTeacher = new AcademicTeacher()
        {
            Id = Guid.CreateVersion7(),
            UsosId = usosId,
            Slug = AcademicTeacherSlug.Create(name, usosId),

            _degrees = degreeList,
            Name = name,
            PhotoUrl = photoUrl,

            Email = email,
            PhoneNumber = phoneNumber,
            WebsiteUrl = websiteUrl,
            Description = description
        };

        return academicTeacher;
    }
}