namespace Application.Features.AcademicTeachers.Queries;

public sealed record AcademicTeacherResponse(
    string Id,
    string Slug,
    AcademicTeacherBaseInfoDto BaseInfo,
    AcademicTeacherContactInfoDto ContactInfo,
    string? Description
    );

public sealed record AcademicTeacherBaseInfoDto(
    string Name,
    string Title,
    string? PhotoUrl
    );

public sealed record AcademicTeacherContactInfoDto(
    string Email,
    string? PhoneNumber,
    string? WebsiteUrl
    );