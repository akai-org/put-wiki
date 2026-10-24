using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Abstractions;
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

public class ProvisionUserCommandHandlerTest
{
    private readonly Mock<IUsosOAuthService> _usosOAuthServiceMock;
    private readonly Mock<IUsosIdHasher> _idHasherMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _iUnitOfWorkMock;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly Mock<IUserSessionService> _userSessionServiceMock;
    private readonly ProvisionUserCommandHandler _sut;

    public ProvisionUserCommandHandlerTest()
    {
        _usosOAuthServiceMock = new Mock<IUsosOAuthService>();
        _idHasherMock = new Mock<IUsosIdHasher>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _iUnitOfWorkMock = new Mock<IUnitOfWork>();
        _fakeTimeProvider = new FakeTimeProvider();
        _userSessionServiceMock = new Mock<IUserSessionService>();

        _sut = new ProvisionUserCommandHandler(
            _usosOAuthServiceMock.Object,
            _idHasherMock.Object,
            _userRepositoryMock.Object,
            _iUnitOfWorkMock.Object,
            NullLogger<ProvisionUserCommandHandler>.Instance,
            _fakeTimeProvider,
            _userSessionServiceMock.Object
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
        _iUnitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserAlreadyExistsInDb_ShouldReturnExistingUserAndCreateSession()
    {
        // Arrange
        var cmd = new ProvisionUserCommand("token", "verifier");
        var rawUsosId = "12345";
        var hashedUsosId = "XYZ_HASHED_ID";
        var fakeDate = new DateTimeOffset(2026, 6, 6, 12, 0, 0, TimeSpan.Zero);
        var existingUser = User.Create(hashedUsosId, fakeDate);
        var usosUserDto = new UsosUserDto(rawUsosId);
        var fakeJwtTokenPair = new JwtTokenPair("ACCESS_TOKEN", "REFRESH_TOKEN");

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _userSessionServiceMock
            .Setup(x => x.CreateSession(existingUser.Id))
            .Returns(fakeJwtTokenPair);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("ACCESS_TOKEN");
        result.Value.RefreshToken.Should().Be("REFRESH_TOKEN");

        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _iUnitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExistInDb_ShouldCreateNewUserAndSaveAndReturnTokens()
    {
        // Arrange
        var cmd = new ProvisionUserCommand("token", "verifier");
        var rawUsosId = "567890";
        var hashedUsosId = "ABC_HASHED_ID";
        var usosUserDto = new UsosUserDto(rawUsosId);
        var fakeDate = new DateTimeOffset(2026, 6, 6, 12, 0, 0, TimeSpan.Zero);
        var fakeJwtTokenPair = new JwtTokenPair("ACCESS_TOKEN", "REFRESH_TOKEN");

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(cmd.OauthToken, cmd.OauthVerifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _userSessionServiceMock
            .Setup(x => x.CreateSession(It.IsAny<Guid>()))
            .Returns(fakeJwtTokenPair);

        _fakeTimeProvider.SetUtcNow(fakeDate);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.AccessToken.Should().Be("ACCESS_TOKEN");
        result.Value.RefreshToken.Should().Be("REFRESH_TOKEN");

        _idHasherMock.Verify(x => x.Hash(rawUsosId), Times.Once);
        _userRepositoryMock.Verify(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()), Times.Once);

        _userRepositoryMock.Verify(x => x.Add(It.Is<User>(u => u.HashedUsosId == hashedUsosId && u.JoinedDate == fakeDate)), Times.Once);
        _iUnitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}