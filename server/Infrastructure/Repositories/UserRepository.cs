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

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}