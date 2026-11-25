using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Lecturers;

namespace Application.Features.Lecturers.Queries;

public interface ILecturerQueryService
{
    Task<LecturerResponse?> GetLecturerByIdAsync(Guid id, CancellationToken ct = default);
    Task<LecturerResponse?> GetLecturerBySlugAsync(LecturerSlug slug, CancellationToken ct = default);
}