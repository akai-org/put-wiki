using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.AcademicTeachers;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AcademicTeacherRepository(AppDbContext context) : IAcademicTeacherRepository
{
    public Task<AcademicTeacher?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(id, out var guidId))
            return Task.FromResult<AcademicTeacher?>(null);

        return context.AcademicTeachers.FirstOrDefaultAsync(teacher => teacher.Id == guidId, cancellationToken);
    }

    public Task<AcademicTeacher?> GetByUsosIdAsync(string usosId, CancellationToken cancellationToken = default)
    {
        return context.AcademicTeachers
            .FirstOrDefaultAsync(teacher => teacher.UsosId == usosId, cancellationToken);
    }

    public void Add(AcademicTeacher academicTeacher)
    {
        context.AcademicTeachers.Add(academicTeacher);
    }
}