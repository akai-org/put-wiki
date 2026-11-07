using System;
using System.Diagnostics.CodeAnalysis;

using FluentResults;

namespace Domain.Users;

public partial class User
{
    public required Guid Id { get; init; }
    public required string HashedUsosId { get; init; } = null!;
    public required DateTimeOffset JoinedDate { get; init; }
    public Nickname? Nickname { get; private set; }

    [SetsRequiredMembers]
    private User() { }

    public static User Create(string hashedUsosId, DateTimeOffset joinedDate)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.", nameof(hashedUsosId));

        return new User
        {
            Id = Guid.CreateVersion7(),
            HashedUsosId = hashedUsosId,
            JoinedDate = joinedDate
        };
    }

    public Result UpdateNickname(string nickname)
    {
        var result = Nickname.Create(nickname);
        if (result.IsFailed)
            return Result.Fail(result.Errors);

        Nickname = result.Value;
        return Result.Ok();
    }
}