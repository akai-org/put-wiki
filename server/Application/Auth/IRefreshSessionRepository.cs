using System;
using System.Threading;
using System.Threading.Tasks;

using Domain.Users;

namespace Application.Auth;

public interface IRefreshSessionRepository
{
    Task<RefreshSession?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task<bool> TryRevokeAsync(
        Guid sessionId,
        DateTimeOffset revokedAt,
        Guid? replacedBySessionId,
        CancellationToken cancellationToken = default);
    void Add(RefreshSession session);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}