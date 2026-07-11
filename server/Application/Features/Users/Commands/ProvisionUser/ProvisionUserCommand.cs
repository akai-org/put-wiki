namespace Application.Features.Users.Commands.ProvisionUser;

public record ProvisionUserCommand(string OauthToken, string OauthVerifier);