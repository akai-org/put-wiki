using System;
namespace Application.DTOs;

public record UserDto(string Id, string HashedUsosId, DateTimeOffset JoinedDate);