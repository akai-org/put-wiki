namespace Application.Features.Lecturers.Queries;

public sealed record LecturerResponse(
    string Id,
    string Slug,
    LecturerBaseInfoDto BaseInfo,
    LecturerContactInfoDto ContactInfo,
    string? Description
    );

public sealed record LecturerBaseInfoDto(
    string Name,
    string Title,
    string? PhotoUrl
    );

public sealed record LecturerContactInfoDto(
    string Email,
    string? PhoneNumber,
    string? WebsiteUrl
    );