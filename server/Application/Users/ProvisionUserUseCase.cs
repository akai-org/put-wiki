using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Core;
using Application.DTOs;

using AutoMapper;

using Domain.Users;

using Microsoft.Extensions.Logging;

namespace Application.Users;

public partial class ProvisionUserUseCase(
    IUsosOAuthService usosOAuthService,
    IUsosIdHasher hasher,
    IUserRepository userRepository,
    ILogger<ProvisionUserUseCase> logger,
    IMapper mapper)
{

    public async Task<Result<UserDto>> ExecuteAsync(string oauthToken, string oauthVerifier,
        CancellationToken ct = default)
    {
        var usosResult = await usosOAuthService.HandleCallbackAndGetUserAsync(oauthToken, oauthVerifier, ct);
        if (!usosResult.IsSuccess || string.IsNullOrWhiteSpace(usosResult.Value?.Id))
        {
            var errorMsg = usosResult.Error ?? "Unknown USOS authentication error.";
            LogProvisioningAbortedUsosAuthenticationFailed(errorMsg);

            var statusCode = usosResult.Code is > 0 and < 600 ? usosResult.Code : 400;
            return Result.Failure<UserDto>(errorMsg, statusCode);
        }

        var rawUsosId = usosResult.Value.Id;
        var hashedId = hasher.Hash(rawUsosId);

        try
        {
            var existingUser = await userRepository.GetByHashedUsosIdAsync(hashedId, ct);
            if (existingUser != null)
            {
                return Result.Success(mapper.Map<UserDto>(existingUser));
            }

            var newUser = new User(hashedId);
            userRepository.Add(newUser);
            await userRepository.SaveChangesAsync(ct);

            LogProvisionedNewAnonymousUserId(newUser.Id);

            return Result.Success(mapper.Map<UserDto>(newUser));
        }
        catch (Exception ex)
        {
            LogFailedToProvisionUserDueToDatabaseError(ex);
            return Result.Failure<UserDto>("Internal database error during user provisioning.", 500);
        }
    }

    [LoggerMessage(LogLevel.Warning, "Provisioning aborted: USOS authentication failed. Error: {error}")]
    partial void LogProvisioningAbortedUsosAuthenticationFailed(string error);

    [LoggerMessage(LogLevel.Information, "Provisioned new anonymous user {userId}")]
    partial void LogProvisionedNewAnonymousUserId(Guid userId);

    [LoggerMessage(LogLevel.Error, "Failed to provision user due to database error.")]
    partial void LogFailedToProvisionUserDueToDatabaseError(Exception ex);
}