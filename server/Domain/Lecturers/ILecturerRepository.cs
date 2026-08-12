using System.Threading;
using System.Threading.Tasks;

namespace Domain.Lecturers;

public interface ILecturerRepository
{
    Task<Lecturer?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    void Add(Lecturer lecturer);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}