using FluentResults;

namespace Domain.Users;

public class NicknameTooShortError(int minLength)
    : Error($"Nickname must be at least {minLength} characters long.");

public class NicknameTooLongError(int maxLength)
    : Error($"Nickname must be at most {maxLength} characters long.");

public class NicknameInvalidFormatError()
    : Error("Nickname can only contain letters, digits, spaces, hyphens and underscores. It must start and end with a letter or digit, and cannot contain consecutive spaces.");