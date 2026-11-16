using System.Text.RegularExpressions;

using FluentResults;

namespace Domain.Users;

public partial record Nickname
{
    public const int MinLength = 3;
    public const int MaxLength = 30;

    public string Value { get; }

    private Nickname(string value)
    {
        Value = value;
    }

    public static Result<Nickname> Create(string input)
    {
        var trimmed = input.Trim();

        if (trimmed.Length < MinLength)
            return Result.Fail(new NicknameTooShortError(MinLength));

        if (trimmed.Length > MaxLength)
            return Result.Fail(new NicknameTooLongError(MaxLength));

        if (!FormatRegex().IsMatch(trimmed))
            return Result.Fail(new NicknameInvalidFormatError());

        return Result.Ok(new Nickname(trimmed));
    }

    // Letters (Unicode) and digits at start/end; letters, digits, spaces, hyphens, underscores in the middle.
    // No consecutive spaces.
    [GeneratedRegex(@"^[\p{L}\p{N}]([\p{L}\p{N}_-]| (?! ))*[\p{L}\p{N}]$")]
    private static partial Regex FormatRegex();
}