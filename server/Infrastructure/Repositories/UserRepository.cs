using System.Threading;
using System.Threading.Tasks;

using Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByHashedUsosIdAsync(string hashedUsosId, CancellationToken cancellationToken = default)
    {
        return _context.Users.SingleOrDefaultAsync(u => u.HashedUsosId == hashedUsosId, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}