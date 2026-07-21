using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Errors;

using AutoMapper;

using Domain.Users;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace Application.Features.Users.Commands.ProvisionUser;

public partial class ProvisionUserUseCase(
    IUsosOAuthService usosOAuthService,
    IJwtService jwtService,
    IUsosIdHasher hasher,
    IUserRepository userRepository,
    ILogger<ProvisionUserUseCase> logger,
    TimeProvider timeProvider)
{
    public async Task<Result<AuthTokenDto>> ExecuteAsync(ProvisionUserCommand cmd,
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

        var existingUser = await userRepository.GetByHashedUsosIdAsync(hashedId, ct);
        User user;
        
        if (existingUser != null)
        {
            user = existingUser;
        }
        else
        {
            user = new User(hashedId, timeProvider.GetUtcNow());
            userRepository.Add(user);
            await userRepository.SaveChangesAsync(ct);
            LogProvisionedNewAnonymousUserId(user.Id);
        }

        var tokenResult = await jwtService.GenerateTokenAsync(user.Id, ct);
        if (tokenResult.IsFailed)
        {
            LogTokenGenerationFailed();
            return Result.Fail(tokenResult.Errors);
        }

        var authToken = new AuthTokenDto(tokenResult.Value, user.Id.ToString(), user.HashedUsosId);
        return Result.Ok(authToken);
    }

    [LoggerMessage(LogLevel.Warning, "Provisioning aborted: USOS authentication failed. Error: {error}")]
    partial void LogProvisioningAbortedUsosAuthenticationFailed(string error);

    [LoggerMessage(LogLevel.Information, "Provisioned new anonymous user {userId}")]
    partial void LogProvisionedNewAnonymousUserId(Guid userId);

    [LoggerMessage(LogLevel.Error, "Failed to generate JWT token during user provisioning")]
    partial void LogTokenGenerationFailed();
}