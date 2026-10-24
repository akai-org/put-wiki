using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Abstractions;
using Application.Auth;
using Application.Errors;

using Domain.Users;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.ProvisionUser;

public partial class ProvisionUserCommandHandler(
    IUsosOAuthService usosOAuthService,
    IUsosIdHasher hasher,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<ProvisionUserCommandHandler> logger,
    TimeProvider timeProvider,
    IUserSessionService userSessionService)
{

    public async Task<Result<LoginResponse>> ExecuteAsync(ProvisionUserCommand cmd,
        CancellationToken ct = default)
    {
        var usosResult = await usosOAuthService.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, ct);
        if (usosResult.IsFailed)
        {
            LogProvisioningAbortedUsosAuthenticationFailed(usosResult.Errors[0].Message);
            return Result.Fail(usosResult.Errors);
        }

        if (string.IsNullOrWhiteSpace(usosResult.Value?.Id))
        {
            return Result.Fail(new ExternalServiceError("USOS returned an empty user ID."));
        }

        var rawUsosId = usosResult.Value.Id;
        var hashedId = hasher.Hash(rawUsosId);

        var user = await userRepository.GetByHashedUsosIdAsync(hashedId, ct);
        if (user is null)
        {
            user = User.Create(hashedId, timeProvider.GetUtcNow());
            userRepository.Add(user);
            LogProvisionedNewUser(user.Id);
        }
        else
        {
            LogUserLoggedIn(user.Id);
        }

        var tokens = userSessionService.CreateSession(user.Id);

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(new LoginResponse(tokens.AccessToken, tokens.RefreshToken));
    }

    [LoggerMessage(LogLevel.Warning, "Provisioning aborted: USOS authentication failed. Error: {error}")]
    partial void LogProvisioningAbortedUsosAuthenticationFailed(string error);

    [LoggerMessage(LogLevel.Information, "Provisioned new user with id: {userId}")]
    partial void LogProvisionedNewUser(Guid userId);

    [LoggerMessage(LogLevel.Information, "User with id: {userId} has logged in")]
    partial void LogUserLoggedIn(Guid userId);
}