using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Domain.AcademicTeachers;

public class AcademicTeacher
{
    public Guid Id { get; private set; }
    public string UsosId { get; private set; }
    public AcademicTeacherSlug Slug { get; private set; }


    [SuppressMessage("Style", "IDE0044:Dodaj modyfikator tylko do odczytu")] //temporary before using field in code
    private List<string> _degrees;
    public IReadOnlyList<string> Degrees => _degrees.AsReadOnly();
    public string Name { get; private set; }
    public string? PhotoUrl { get; private set; }

    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? Description { get; private set; }

    public AcademicTeacher(
        string usosId,
        IReadOnlyList<string> degrees,
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

        Id = Guid.CreateVersion7();
        UsosId = usosId;
        Slug = AcademicTeacherSlug.From(name, usosId);

        _degrees = degreeList;
        Name = name;
        PhotoUrl = photoUrl;

        Email = email;
        PhoneNumber = phoneNumber;
        WebsiteUrl = websiteUrl;
        Description = description;
    }
}