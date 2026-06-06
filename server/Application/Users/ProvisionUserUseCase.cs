using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.DTOs;
using Application.Errors;

using AutoMapper;

using Domain.Users;

using FluentResults;

using Microsoft.Extensions.Logging;

namespace Application.Users;

public partial class ProvisionUserUseCase(
    IUsosOAuthService usosOAuthService,
    IUsosIdHasher hasher,
    IUserRepository userRepository,
    ILogger<ProvisionUserUseCase> logger,
    IMapper mapper,
    TimeProvider timeProvider)
{

    public async Task<Result<UserDto>> ExecuteAsync(string oauthToken, string oauthVerifier,
        CancellationToken ct = default)
    {
        var usosResult = await usosOAuthService.HandleCallbackAndGetUserAsync(oauthToken, oauthVerifier, ct);
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
        if (existingUser != null)
        {
            return Result.Ok(mapper.Map<UserDto>(existingUser));
        }

        var newUser = new User(hashedId, timeProvider.GetUtcNow());
        userRepository.Add(newUser);
        await userRepository.SaveChangesAsync(ct);

        LogProvisionedNewAnonymousUserId(newUser.Id);

        return Result.Ok(mapper.Map<UserDto>(newUser));
    }

    [LoggerMessage(LogLevel.Warning, "Provisioning aborted: USOS authentication failed. Error: {error}")]
    partial void LogProvisioningAbortedUsosAuthenticationFailed(string error);

    [LoggerMessage(LogLevel.Information, "Provisioned new anonymous user {userId}")]
    partial void LogProvisionedNewAnonymousUserId(Guid userId);
}