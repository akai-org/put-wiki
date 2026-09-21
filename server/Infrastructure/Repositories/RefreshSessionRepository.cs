using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;

using Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshSessionRepository(AppDbContext context) : IRefreshSessionRepository
{
    public Task<RefreshSession?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken = default)
    {
        return context.RefreshSessions
            .SingleOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }

    public void Add(RefreshSession session)
    {
        context.RefreshSessions.Add(session);
    }

    public async Task<bool> TryRevokeAsync(
        Guid sessionId,
        DateTimeOffset revokedAt,
        Guid? replacedBySessionId,
        CancellationToken cancellationToken = default)
    {
        var updatedRows = await context.RefreshSessions
            .Where(session => session.Id == sessionId && session.RevokedAt == null)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(session => session.RevokedAt, revokedAt)
                    .SetProperty(session => session.ReplacedBySessionId, replacedBySessionId),
                cancellationToken);

        return updatedRows == 1;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}