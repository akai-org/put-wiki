using System.Threading;
using System.Threading.Tasks;

namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByHashedUsosIdAsync(string hashedUsosId, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}