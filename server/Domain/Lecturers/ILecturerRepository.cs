using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Lecturers;

public interface ILecturerRepository
{
    Task<Lecturer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Lecturer?> GetByUsosIdAsync(string usosId, CancellationToken cancellationToken = default);
    void Add(Lecturer lecturer);
}