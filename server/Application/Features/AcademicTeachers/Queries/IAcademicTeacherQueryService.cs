using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AcademicTeachers.Queries;

public interface IAcademicTeacherQueryService
{
    Task<AcademicTeacherResponse?> GetAcademicTeacherByIdAsync(Guid id, CancellationToken ct = default);
}