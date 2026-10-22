namespace Application.Features.Users.Commands.ProvisionUser;

public sealed record LoginResponse(string AccessToken, string RefreshToken);