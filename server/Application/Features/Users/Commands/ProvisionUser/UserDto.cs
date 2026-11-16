using System;

namespace Application.Features.Users.Commands.ProvisionUser;

public record UserDto(string Id, string HashedUsosId, DateTimeOffset JoinedDate);