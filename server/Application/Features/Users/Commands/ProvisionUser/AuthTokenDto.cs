namespace Application.Features.Users.Commands.ProvisionUser;

public record AuthTokenDto(string Token, string UserId, string HashedUsosId);
