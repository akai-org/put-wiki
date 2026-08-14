using System;
using System.Text.RegularExpressions;

namespace Domain.AcademicTeachers;

public partial record AcademicTeacherSlug
{
    public string Value { get; init; }

    private static readonly Regex SlugRegex = MySlugRegex();

    private AcademicTeacherSlug(string value)
    {
        Value = value;
    }

    public static AcademicTeacherSlug From(string name, string usosId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(usosId);

        var slugifiedName = Slugify(name);
        var slugifiedUsosId = Slugify(usosId);

        return new AcademicTeacherSlug($"{slugifiedName}-{slugifiedUsosId}");
    }

    public static AcademicTeacherSlug Parse(string slug)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(slug);

        if (!SlugRegex.IsMatch(slug))
        {
            throw new ArgumentException($"The provided string '{slug}' is not a valid slug format.", nameof(slug));
        }

        return new AcademicTeacherSlug(slug);
    }

    public override string ToString() => Value;

    private static string Slugify(string text)
    {
        var str = text.ToLowerInvariant();

        str = str.Replace("ą", "a").Replace("ć", "c").Replace("ę", "e")
            .Replace("ł", "l").Replace("ń", "n").Replace("ó", "o")
            .Replace("ś", "s").Replace("ź", "z").Replace("ż", "z");

        str = InvalidCharsRegex().Replace(str, "");
        str = HyphensAndSpacesRegex().Replace(str.Trim(), "-");

        return str;
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex InvalidCharsRegex();

    [GeneratedRegex(@"[\s-]+")]
    private static partial Regex HyphensAndSpacesRegex();

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex MySlugRegex();
}