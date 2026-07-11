using System.Threading;
using System.Threading.Tasks;

using Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{

    public Task<User?> GetByHashedUsosIdAsync(string hashedUsosId, CancellationToken cancellationToken = default)
    {
        return context.Users.SingleOrDefaultAsync(u => u.HashedUsosId == hashedUsosId, cancellationToken);
    }

    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}