using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Lecturers;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LecturerRepository(AppDbContext context) : ILecturerRepository
{
    public Task<Lecturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Lecturers.FirstOrDefaultAsync(lecturer => lecturer.Id == id, cancellationToken);
    }

    public Task<Lecturer?> GetByUsosIdAsync(string usosId, CancellationToken cancellationToken = default)
    {
        return context.Lecturers
            .FirstOrDefaultAsync(lecturer => lecturer.UsosId == usosId, cancellationToken);
    }

    public void Add(Lecturer lecturer)
    {
        context.Lecturers.Add(lecturer);
    }
}