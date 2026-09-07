using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.AcademicTeachers;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AcademicTeacherRepository(AppDbContext context) : IAcademicTeacherRepository
{
    public Task<AcademicTeacher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.AcademicTeachers.FirstOrDefaultAsync(teacher => teacher.Id == id, cancellationToken);
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