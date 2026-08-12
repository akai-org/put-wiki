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
            .Select(t => new AcademicTeacherDto(t.Name))
            .FirstOrDefaultAsync((ct));
    }
}