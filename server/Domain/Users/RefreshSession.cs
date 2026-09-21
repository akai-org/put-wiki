using System;

namespace Domain.Users;

public class RefreshSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public Guid? ReplacedBySessionId { get; private set; }

    public User User { get; private set; } = null!;

    private RefreshSession()
    {
        TokenHash = string.Empty;
    }

    public RefreshSession(
        Guid userId,
        string tokenHash,
        DateTimeOffset createdAt,
        DateTimeOffset expiresAt)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }

    public bool IsActive(DateTimeOffset now) => RevokedAt is null && ExpiresAt > now;

    public void Revoke(DateTimeOffset revokedAt, Guid? replacedBySessionId = null)
    {
        RevokedAt ??= revokedAt;
        ReplacedBySessionId ??= replacedBySessionId;
    }
}