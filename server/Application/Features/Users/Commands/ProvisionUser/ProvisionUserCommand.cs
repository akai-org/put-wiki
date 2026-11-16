namespace Application.Features.Users.Commands.ProvisionUser;

public sealed record ProvisionUserCommand(string OauthToken, string OauthVerifier);