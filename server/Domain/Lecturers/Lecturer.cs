using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Lecturers;

public class Lecturer
{
    public Guid Id { get; private set; }
    public string UsosId { get; private set; }

    private readonly List<string> _degrees;
    public IReadOnlyList<string> Degrees => _degrees.AsReadOnly();
    public string Name { get; private set; }
    public string? PhotoUrl { get; private set; }

    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? WebsiteUrl { get; private set; }

    public Lecturer(
        string usosId,
        IEnumerable<string> degrees,
        string name,
        string email,
        string? photoUrl = null,
        string? phoneNumber = null,
        string? websiteUrl = null)
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
        Name = name;
        Email = email;
        PhotoUrl = photoUrl;
        PhoneNumber = phoneNumber;
        WebsiteUrl = websiteUrl;
        _degrees = degreeList;
    }
}