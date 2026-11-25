using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.Lecturers.Queries;

using Domain.Lecturers;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class LecturerQueryService(AppDbContext context) : ILecturerQueryService
{
    public async Task<LecturerResponse?> GetLecturerByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Project(context.Lecturers.AsNoTracking().Where(lecturer => lecturer.Id == id))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<LecturerResponse?> GetLecturerBySlugAsync(LecturerSlug slug, CancellationToken ct = default)
    {
        return await Project(context.Lecturers.AsNoTracking().Where(lecturer => lecturer.Slug == slug))
            .FirstOrDefaultAsync(ct);
    }

    private static IQueryable<LecturerResponse> Project(IQueryable<Lecturer> lecturers)
    {
        return lecturers.Select(lecturer => new LecturerResponse(
                lecturer.Id.ToString(),
                lecturer.Slug.ToString(),
              new LecturerBaseInfoDto(
                  lecturer.Name,
                  string.Join(" ", lecturer.Degrees),
                  lecturer.PhotoUrl
              ),
              new LecturerContactInfoDto(
                  lecturer.Email,
                  lecturer.PhoneNumber,
                  lecturer.WebsiteUrl
              ),
              lecturer.Description
            ));
    }
}