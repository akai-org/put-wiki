using System.Text.Json.Serialization;

namespace Application.Features.AcademicTeachers.Queries;

public record AcademicTeacherDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("base_info")] AcademicTeacherBaseInfoDto BaseInfo,
    [property: JsonPropertyName("contact")] AcademicTeacherContactInfoDto ContactInfo
    );

public record AcademicTeacherBaseInfoDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("photo_url")] string? PhotoUrl
    );

public record AcademicTeacherContactInfoDto(
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("phone")] string? PhoneNumber,
    [property: JsonPropertyName("website_url")] string? WebsiteUrl
    );