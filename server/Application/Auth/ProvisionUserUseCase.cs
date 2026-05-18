using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Core;

using Domain.Users;

using Microsoft.Extensions.Logging;

namespace Application.Auth;

public partial class ProvisionUserUseCase
{
    private readonly IUsosOAuthService _usosOAuthService;
    private readonly IUsosIdHasher _hasher;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ProvisionUserUseCase> _logger;

    public ProvisionUserUseCase(
        IUsosOAuthService usosOAuthService,
        IUsosIdHasher hasher,
        IUserRepository userRepository,
        ILogger<ProvisionUserUseCase> logger)
    {
        _usosOAuthService = usosOAuthService;
        _hasher = hasher;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result<User>> ExecuteAsync(string oauthToken, string oauthVerifier,
        CancellationToken ct = default)
    {
        var usosResult = await _usosOAuthService.HandleCallbackAndGetUserAsync(oauthToken, oauthVerifier, ct);
        if (!usosResult.IsSuccess || string.IsNullOrWhiteSpace(usosResult.Value?.Id))
        {
            var errorMsg = usosResult.Error ?? "Unknown USOS authentication error.";
            LogProvisioningAbortedUsosAuthenticationFailed(errorMsg);

            var statusCode = usosResult.Code is > 0 and < 600 ? usosResult.Code : 400;
            return Result.Failure<User>(errorMsg, statusCode);
        }

        var rawUsosId = usosResult.Value.Id;
        var hashedId = _hasher.Hash(rawUsosId);

        try
        {
            var existingUser = await _userRepository.GetByHashedUsosIdAsync(hashedId, ct);
            if (existingUser != null)
            {
                return Result.Success(existingUser);
            }

            var newUser = new User(hashedId);
            await _userRepository.AddAsync(newUser, ct);
            await _userRepository.SaveChangesAsync(ct);

            LogProvisionedNewAnonymousUserUserid(newUser.Id);
            return Result.Success(newUser);
        }
        catch (Exception ex)
        {
            LogFailedToProvisionUserDueToDatabaseError(ex);
            return Result.Failure<User>("Internal database error during user provisioning.", 500);
        }
    }

    [LoggerMessage(LogLevel.Warning, "Provisioning aborted: USOS authentication failed. Error: {error}")]
    partial void LogProvisioningAbortedUsosAuthenticationFailed(string error);

    [LoggerMessage(LogLevel.Information, "Provisioned new anonymous user {userId}")]
    partial void LogProvisionedNewAnonymousUserUserid(Guid userId);

    [LoggerMessage(LogLevel.Error, "Failed to provision user due to database error.")]
    partial void LogFailedToProvisionUserDueToDatabaseError(Exception ex);
}