using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;

using Domain.Lecturers;

using FluentResults;

namespace Application.Features.Lecturers.Queries;

public class GetLecturerQueryHandler(ILecturerQueryService queryService)
{
    public async Task<Result<LecturerResponse>> ExecuteAsync(GetLecturerQuery query,
        CancellationToken ct = default)
    {
        LecturerResponse? lecturerDto;
        if (Guid.TryParse(query.Identifier, out var id))
        {
            lecturerDto = await queryService.GetLecturerByIdAsync(id, ct);
        }
        else if (LecturerSlug.TryParse(query.Identifier, out var slug))
        {
            lecturerDto = await queryService.GetLecturerBySlugAsync(slug, ct);
        }
        else
        {
            return Result.Fail(new ValidationError("Provided lecturer identifier is invalid."));
        }

        if (lecturerDto == null)
            return Result.Fail(new NotFoundError($"Lecturer with identifier {query.Identifier} was not found."));

        return Result.Ok(lecturerDto);
    }
}