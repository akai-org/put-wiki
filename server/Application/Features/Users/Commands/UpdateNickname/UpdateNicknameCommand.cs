using System;

namespace Application.Features.Users.Commands.UpdateNickname;

public sealed record UpdateNicknameCommand(Guid UserId, string Nickname);