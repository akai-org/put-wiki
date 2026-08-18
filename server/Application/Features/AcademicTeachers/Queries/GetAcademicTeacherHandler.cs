using System.Threading;
using System.Threading.Tasks;

using Application.Errors;

using FluentResults;

namespace Application.Features.AcademicTeachers.Queries;

public class GetAcademicTeacherHandler(IAcademicTeacherQueryService queryService)
{
    public async Task<Result<AcademicTeacherDto>> ExecuteAsync(GetAcademicTeacherQuery query,
        CancellationToken ct = default)
    {
        var teacherDto = await queryService.GetAcademicTeacherByIdAsync(query.Id, ct);

        if (teacherDto == null)
            return Result.Fail(new NotFoundError($"Academic teacher with ID {query.Id} was not found."));

        return Result.Ok(teacherDto);
    }
}