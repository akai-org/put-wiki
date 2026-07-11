using System.Threading;
using System.Threading.Tasks;

namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByHashedUsosIdAsync(string hashedUsosId, CancellationToken cancellationToken = default);
    void Add(User user);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}