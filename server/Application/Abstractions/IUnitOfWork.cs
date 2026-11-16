using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}