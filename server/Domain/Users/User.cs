using System;
using System.Text.RegularExpressions;

using FluentResults;

namespace Domain.Users;

public partial class User
{
    public const int MinNicknameLength = 3;
    public const int MaxNicknameLength = 30;

    public Guid Id { get; private set; }
    public string HashedUsosId { get; private set; }
    public DateTimeOffset JoinedDate { get; init; }
    public string? Nickname { get; private set; }

    public User(string hashedUsosId, DateTimeOffset joinedDate)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.");

        Id = Guid.CreateVersion7();
        HashedUsosId = hashedUsosId;
        JoinedDate = joinedDate;
    }

    // Required by EF Core for entity materialization
    private User() { HashedUsosId = null!; }

    public Result UpdateNickname(string nickname)
    {
        var trimmed = nickname.Trim();

        if (trimmed.Length < MinNicknameLength)
            return Result.Fail(new NicknameTooShortError(MinNicknameLength));

        if (trimmed.Length > MaxNicknameLength)
            return Result.Fail(new NicknameTooLongError(MaxNicknameLength));

        if (!NicknameFormatRegex().IsMatch(trimmed))
            return Result.Fail(new NicknameInvalidFormatError());

        Nickname = trimmed;
        return Result.Ok();
    }

    // Letters (Unicode) and digits at start/end; letters, digits, spaces, hyphens, underscores in the middle.
    // No consecutive spaces.
    [GeneratedRegex(@"^[\p{L}\p{N}]([\p{L}\p{N}_-]| (?! ))*[\p{L}\p{N}]$")]
    private static partial Regex NicknameFormatRegex();
}