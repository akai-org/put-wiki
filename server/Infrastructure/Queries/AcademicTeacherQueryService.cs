using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Features.AcademicTeachers.Queries;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries;

public class AcademicTeacherQueryService(AppDbContext context) : IAcademicTeacherQueryService
{
    public async Task<AcademicTeacherDto?> GetAcademicTeacherByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.AcademicTeachers
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new AcademicTeacherDto(
                t.Id.ToString(),
              new AcademicTeacherBaseInfoDto(
                  t.Name,
                  string.Join(" ", t.Degrees),
                  t.PhotoUrl
              ),
              new AcademicTeacherContactInfoDto(
                  t.Email,
                  t.PhoneNumber,
                  t.WebsiteUrl
              )
            ))
            .FirstOrDefaultAsync((ct));
    }
}