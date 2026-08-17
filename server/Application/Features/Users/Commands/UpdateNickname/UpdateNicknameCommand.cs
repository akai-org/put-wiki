using System;

namespace Application.Features.Users.Commands.UpdateNickname;

public record UpdateNicknameCommand(Guid UserId, string Nickname);
