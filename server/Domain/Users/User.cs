using System;

using FluentResults;
namespace Domain.Users;

public partial class User
{
    public const int MinNicknameLength = 3;
    public const int MaxNicknameLength = 30;

    public required Guid Id { get; init; }
    public required string HashedUsosId { get; init; }
    public required DateTimeOffset JoinedDate { get; init; }
    public Nickname? Nickname { get; private set; }

    public static User Create(string hashedUsosId, DateTimeOffset joinedDate)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.");

        var user = new User()
        {
            Id = Guid.CreateVersion7(),
            HashedUsosId = hashedUsosId,
            JoinedDate = joinedDate
        };

        return user;
    }

    // Required by EF Core for entity materialization
    private User() { }

    public Result UpdateNickname(string nickname)
    {
        var result = Nickname.Create(nickname);
        if (result.IsFailed)
            return Result.Fail(result.Errors);

        Nickname = result.Value;
        return Result.Ok();
    }
}