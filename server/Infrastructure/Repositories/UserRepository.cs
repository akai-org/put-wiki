using System;
using System.Linq;
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

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<bool> ExistsWithNicknameAsync(string nickname, Guid excludedUserId, CancellationToken cancellationToken = default)
    {
        return context.Users.AnyAsync(
            u => u.Id != excludedUserId && u.Nickname != null && EF.Functions.ILike(u.Nickname, nickname),
            cancellationToken);
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