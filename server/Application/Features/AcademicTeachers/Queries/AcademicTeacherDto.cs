namespace Application.Features.AcademicTeachers.Queries;

public record AcademicTeacherDto(
    string Id,
    string Slug,
    AcademicTeacherBaseInfoDto BaseInfo,
    AcademicTeacherContactInfoDto ContactInfo,
    string? Description
    );

public record AcademicTeacherBaseInfoDto(
    string Name,
    string Title,
    string? PhotoUrl
    );

public record AcademicTeacherContactInfoDto(
    string Email,
    string? PhoneNumber,
    string? WebsiteUrl
    );