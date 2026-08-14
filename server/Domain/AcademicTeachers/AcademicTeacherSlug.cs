using System;
using System.Text.RegularExpressions;

namespace Domain.AcademicTeachers;

public record AcademicTeacherSlug
{
    public string Value { get; }

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

    public override string ToString() => Value;

    private static string Slugify(string text)
    {
        var str = text.ToLowerInvariant();

        str = str.Replace("ą", "a").Replace("ć", "c").Replace("ę", "e")
            .Replace("ł", "l").Replace("ń", "n").Replace("ó", "o")
            .Replace("ś", "s").Replace("ź", "z").Replace("ż", "z");

        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = Regex.Replace(str, @"\s", "-");

        return str;
    }
}