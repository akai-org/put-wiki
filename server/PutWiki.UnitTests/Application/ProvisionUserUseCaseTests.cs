using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Errors;
using Application.Features.Users.Commands.ProvisionUser;

using Domain.Users;

using FluentAssertions;

using FluentResults;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;

using Moq;

namespace PutWiki.UnitTests.Application;

public class ProvisionUserUseCaseTests
{
    private readonly Mock<IUsosOAuthService> _usosOAuthServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IUsosIdHasher> _idHasherMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly ProvisionUserUseCase _sut;

    public ProvisionUserUseCaseTests()
    {
        _usosOAuthServiceMock = new Mock<IUsosOAuthService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _idHasherMock = new Mock<IUsosIdHasher>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _fakeTimeProvider = new FakeTimeProvider();

        _sut = new ProvisionUserUseCase(
            _usosOAuthServiceMock.Object,
            _jwtServiceMock.Object,
            _idHasherMock.Object,
            _userRepositoryMock.Object,
            NullLogger<ProvisionUserUseCase>.Instance,
            _fakeTimeProvider
        );
    }

    [Fact]
    public async Task ExecuteAsync_WhenUsosAuthenticationFails_ShouldReturnFailureResult()
    {
        // Arrange
        var cmd = new ProvisionUserCommand("token", "verifier");
        var expectedError = "Invalid credentials";

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new UnauthorizedError(expectedError)));

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be(expectedError);

        _idHasherMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _jwtServiceMock.Verify(x => x.GenerateTokenAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserAlreadyExistsInDb_ShouldReturnExistingUserWithoutCreatingNewOne()
    {
        // Arrange
        var cmd = new ProvisionUserCommand("token", "verifier");
        var rawUsosId = "12345";
        var hashedUsosId = "XYZ_HASHED_ID";
        var fakeDate = new DateTimeOffset(2026, 6, 6, 12, 0, 0, TimeSpan.Zero);
        var existingUser = new User(hashedUsosId, fakeDate);
        var usosUserDto = new UsosUserDto(rawUsosId);

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);
        _jwtServiceMock
            .Setup(x => x.GenerateTokenAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok("token"));

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("token");

        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExistInDb_ShouldCreateNewUserAndSaveToDb()
    {
        // Arrange
        var cmd = new ProvisionUserCommand("token", "verifier");
        var rawUsosId = "567890";
        var hashedUsosId = "ABC_HASHED_ID";
        var usosUserDto = new UsosUserDto(rawUsosId);
        var fakeDate = new DateTimeOffset(2026, 6, 6, 12, 0, 0, TimeSpan.Zero);
        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _jwtServiceMock
            .Setup(x => x.GenerateTokenAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid userId, CancellationToken _) => Result.Ok($"token-{userId}"));
        _fakeTimeProvider.SetUtcNow(fakeDate);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().StartWith("token-");

        _userRepositoryMock.Verify(x => x.Add(It.Is<User>(u => u.HashedUsosId == hashedUsosId && u.JoinedDate == fakeDate)), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}