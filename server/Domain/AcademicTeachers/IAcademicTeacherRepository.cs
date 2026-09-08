using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.AcademicTeachers;

public interface IAcademicTeacherRepository
{
    Task<AcademicTeacher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AcademicTeacher?> GetByUsosIdAsync(string usosId, CancellationToken cancellationToken = default);
    void Add(AcademicTeacher academicTeacher);
}