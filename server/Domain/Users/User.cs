using System;

using FluentResults;

namespace Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string HashedUsosId { get; private set; }
    public DateTimeOffset JoinedDate { get; init; }
    public Nickname? Nickname { get; private set; }

    public User(string hashedUsosId, DateTimeOffset joinedDate)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.");

        Id = Guid.CreateVersion7();
        HashedUsosId = hashedUsosId;
        JoinedDate = joinedDate;
        Nickname = null;
    }

    // Required by EF Core for entity materialization
    private User() { HashedUsosId = null!; }

    public Result UpdateNickname(string nickname)
    {
        var result = Users.Nickname.Create(nickname);
        if (result.IsFailed)
            return Result.Fail(result.Errors);

        Nickname = result.Value;
        return Result.Ok();
    }
}